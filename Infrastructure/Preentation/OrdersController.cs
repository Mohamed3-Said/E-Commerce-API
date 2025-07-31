namespace Presentation
{
    public class OrdersController(IServiceManager _serviceManager) : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<OrderRequest>> CreateOrder(OrderRequest request)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _serviceManager.OrderService.CreateOrderAsync(request, email);
            return Ok(order);
        }
        [HttpGet("AllOrders")]
        public async Task<ActionResult<IEnumerable<OrderRequest>>> GetAllOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _serviceManager.OrderService.GetAllOrdersByEmailAsync(email);
            return Ok(orders);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderRequest>> GetOrderById(Guid id)
        {
            var order = await _serviceManager.OrderService.GetOrderByIdAsync(id);
            return Ok(order);
        }
        [HttpGet("DeliveryMethod")]
        public async Task<ActionResult> GetDeliveryMethods()
        {
            return Ok(await _serviceManager.OrderService.GetDeliveryMethodsAsync());
        }
    }
}
