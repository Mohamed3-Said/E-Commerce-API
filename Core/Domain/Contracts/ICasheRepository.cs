namespace Domain.Contracts
{
    public interface ICasheRepository
    {
        Task SetAsync(string key, object value, TimeSpan duration);

        Task<string?> GetAsync(string key);
    }
}
