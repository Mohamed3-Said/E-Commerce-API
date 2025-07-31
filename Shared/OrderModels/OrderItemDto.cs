namespace Shared.OrderModels
{
    public record OrderItemDto
    {
        public int ProductId { get; init; }
        public string ProductName { get; init; }
        public string PictureUrl { get; init; }
        public int Quantity { get; init; }
        public double Price { get; init; }
    }
}
