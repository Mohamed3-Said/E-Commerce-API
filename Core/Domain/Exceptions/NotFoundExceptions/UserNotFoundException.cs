namespace Domain.Exceptions.NotFoundExceptions
{
    public sealed class UserNotFoundException(string email) : NotFoundException($"The user with email {email} is not found")
    {
    }
}
