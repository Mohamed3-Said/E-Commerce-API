namespace Domain.Exceptions.NotFoundExceptions
{
    public sealed class DeliveryMethodNotFoundException(int Id) : NotFoundException($"The delivery method with {Id} is not found")
    {
    }
}
