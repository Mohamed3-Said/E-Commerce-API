namespace Domain.Exceptions.NotFoundExceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string msg) : base(msg)
        { }
    }
}
