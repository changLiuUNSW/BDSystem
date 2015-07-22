
namespace DateAccess.Services.SearchService.Role
{
    /// <summary>
    /// provide Role specific searches
    /// </summary>
    public interface ISearchAdapter
    {
        void Configure<T>(SearchConfiguration<T> configuration) where T : class;
    }
}
