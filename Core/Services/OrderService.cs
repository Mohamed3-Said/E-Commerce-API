namespace Services
{
    public class OrderService(IMapper mapper,IBasketRepository _basketRepository,IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string userEmail)
        {
            var shippingAddress = mapper.Map<ShippingAddress>(orderRequest.ShipToAddress);
            var basket = await _basketRepository.GetBasketAsync(orderRequest.BasketId) ??
                throw new BasketNotFoundException(orderRequest.BasketId);
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id) ??
                    throw new ProductNotFoundException(item.Id);
                orderItems.Add(CreateOrderItem(item,product));
            }
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod,int>()
                .GetByIdAsync(orderRequest.DeliveryMethodId) ?? throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var existingOrder = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(basket.PaymentIntentId));
            if (existingOrder != null)
            {
                orderRepo.Delete(existingOrder);
            }
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            var order = new Order(userEmail, shippingAddress, orderItems, deliveryMethod, subTotal,basket.PaymentIntentId);
            await orderRepo.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return mapper.Map<OrderResult>(order);
        }

        private OrderItem CreateOrderItem(BasketItem item, Product product)
            => new OrderItem(new ProductInOrderItem(product.Id,product.Name,product.PictureUrl),item.Quantity,product.Price);
            
        
        public async Task<IEnumerable<OrderResult>> GetAllOrdersByEmailAsync(string userEmail)
        {
            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(new OrderWithIncludesSpecifications(userEmail));
            return mapper.Map<IEnumerable<OrderResult>>(orders);
        }

        public async Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            var methods= await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            return mapper.Map<IEnumerable<DeliveryMethodResult>>(methods); 
        }

        public async Task<OrderResult> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>()
                .GetByIdAsync(new OrderWithIncludesSpecifications(orderId)) ?? throw new OrderNotFoundException(orderId);
            return mapper.Map<OrderResult>(order);
        }
    }
}
