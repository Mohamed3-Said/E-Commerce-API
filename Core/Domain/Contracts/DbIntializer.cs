namespace Domain.Contracts
{
    public interface IDbIntializer
    {
        Task IntializeAsync();
        Task IntializeIdentityAsync();
    }
}
