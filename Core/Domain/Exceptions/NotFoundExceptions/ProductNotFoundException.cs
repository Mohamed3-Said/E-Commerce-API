namespace Domain.Exceptions.NotFoundExceptions
{
    public sealed class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(int id) : base($"Product with id: {id} not found")
        { }
    }
}
