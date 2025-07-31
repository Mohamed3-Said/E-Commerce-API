namespace E_CommerceG01.Factories
{
    public class ApiResponseFactory
    {
        public static IActionResult CustomValidationErrorResponse(ActionContext context)
        {
            var errors = context.ModelState.Where(error => error.Value!.Errors.Any()).Select(
                error=>new ValidationError
                {
                    Field = error.Key,
                    Errors = error.Value!.Errors.Select(e => e.ErrorMessage)
                }
            );
            var response = new ValidationErrorResponse
            {
                StatusCode = (int) HttpStatusCode.BadRequest,
                Errors = errors,
                ErrorMessage = "Validation Failed"
            };
            return new BadRequestObjectResult(response);
        }
    }
}
