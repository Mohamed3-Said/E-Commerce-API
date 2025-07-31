namespace Services.Abstraction
{
    public interface ICasheService
    {
        Task<string?> GetCasheValueAsync(string key);

        Task SetCasheValueAsync(string key, object value, TimeSpan duration);
    }
}
