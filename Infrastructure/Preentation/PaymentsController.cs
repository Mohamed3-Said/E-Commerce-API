namespace Presentation
{
    public class PaymentsController(IServiceManager serviceManager) : ApiController
    {
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePayment(string basketId)
        {
            var result = await serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            return Ok(result);
        }

        [HttpPost("WebHook")]
        public async Task<ActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripHeaders = Request.Headers["Stripe-Signature"];
            await serviceManager.PaymentService.UpdatePaymentStatusAsync(json, stripHeaders);
            return new EmptyResult(); 
        }
    }
}
