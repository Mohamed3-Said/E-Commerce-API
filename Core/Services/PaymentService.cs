using Stripe.Forwarding;

namespace Services
{
    public class PaymentService(IBasketRepository _basketRepository, IConfiguration _configuration,
                                IUnitOfWork _unitOfWork, IMapper mapper) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration.GetSection("StripeSettings")["SecretKey"];

            var Basket = await _basketRepository.GetBasketAsync(basketId) ??
                throw new BasketNotFoundException(basketId);

            foreach (var item in Basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);

                if (item.Price != product.Price)
                    item.Price = product.Price;
            }


            // Get Shipping cost
            if (!Basket.DeliveryMethodId.HasValue) throw new Exception("No delivery method was selected");

            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(Basket.DeliveryMethodId.Value)
                ?? throw new DeliveryMethodNotFoundException(Basket.DeliveryMethodId.Value);

            Basket.ShippingPrice = deliveryMethod.Price;

            var amount = (long)(Basket.Items.Sum(item => item.Price * item.Quantity) + Basket.ShippingPrice) * 100;

            //Payment Intent service
            var paymentIntentService = new PaymentIntentService();

            if (string.IsNullOrWhiteSpace(Basket.PaymentIntentId))
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>
                    {
                        "card"
                    },
                };
                var paymentIntent = await paymentIntentService.CreateAsync(Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = amount
                };
                await paymentIntentService.UpdateAsync(Basket.PaymentIntentId, Options);
            }
            await _basketRepository.UpdateBasketAsync(Basket);
            return mapper.Map<BasketDto>(Basket);
        }

        public async Task UpdatePaymentStatusAsync(string request, string stripeHeaders)
        {


            var endpointSecret = _configuration.GetSection("StripeSettings")["EndpointSecret"];

            var stripeEvent = EventUtility.ConstructEvent(request,
                stripeHeaders, endpointSecret, throwOnApiVersionMismatch: false);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;


            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                await UpdatePaymentStatusSucceeded(paymentIntent.Id);
            }
            else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                await UpdatePaymentStatusFailed(paymentIntent.Id);
            }

        }

        private async Task UpdatePaymentStatusFailed(string paymentIntentId)
        {
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId));
            order.PaymentStatus = OrderPaymentStatus.PaymentFailed;
            orderRepo.Update(order);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task UpdatePaymentStatusSucceeded(string paymentIntentId)
        {
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId));
            order.PaymentStatus = OrderPaymentStatus.PaymentRecieved;
            orderRepo.Update(order);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
