namespace Shared
{
    public record PaginatedResult<TData>(int pageSize, int pageIndex,int totalCount, IEnumerable<TData> Data)
    {
    }
}
