namespace Services.Specifications
{
    class OrderWithPaymentIntentIdSpecifications: Specifications<Order>
    {
        public OrderWithPaymentIntentIdSpecifications(string paymentIntentId) : base(x => x.PaymentIntentId == paymentIntentId)
        {
        }
    }
}
