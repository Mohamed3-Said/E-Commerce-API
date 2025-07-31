using Shared.OrderModels;

namespace Services.Abstraction
{
    public interface IOrderService 
    {
        Task<OrderResult> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<OrderResult>> GetAllOrdersByEmailAsync(string userEmail);
        Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string userEmail);
        Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync();

    }
}
