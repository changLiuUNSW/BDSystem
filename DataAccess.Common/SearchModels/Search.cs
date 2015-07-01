using System.Collections.Generic;

namespace DataAccess.Common.SearchModels
{
    public class Search
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public string SortColumn { get; set; }
        public string Order { get; set; }
        public string Type { get; set; }
        public List<SearchField> SearchFields { get; set; }
    }
}
