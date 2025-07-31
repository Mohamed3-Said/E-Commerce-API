namespace Shared.OrderModels
{
    public record OrderResult
    {
        public Guid Id { get; init; }
        public string UserEmail { get; init; }
        public AddressDto ShippingAddress { get; init; }
        public ICollection<OrderItemDto> OrderItems { get; init; } = new List<OrderItemDto>();
        public string PaymentStatus { get; init; }
        public string DeliveryMethod { get; init; }
        public decimal SubTotal { get; init; }
        public DateTimeOffset OrderDate { get; init; } = DateTimeOffset.Now;
        public string PaymentIntenId { get; init; } = string.Empty;
        public decimal Total { get; init; }

    }
}
