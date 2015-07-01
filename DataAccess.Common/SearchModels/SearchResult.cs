using System.Collections.Generic;

namespace DataAccess.Common.SearchModels
{
    public class SearchResult<T>
    {
        public int Total { get; set; }
        public List<T> List { get; set; } 
    }
}
