namespace Domain.Exceptions.NotFoundExceptions
{
    public sealed class OrderNotFoundException(Guid id) : NotFoundException($"Order with {id} is not found")
    {
    }
}
