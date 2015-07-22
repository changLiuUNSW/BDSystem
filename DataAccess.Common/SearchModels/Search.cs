using System.Collections.Generic;

namespace DataAccess.Common.SearchModels
{
    public class Search
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public string SortColumn { get; set; }
        public string Order { get; set; }

        /// <summary>
        /// this field affects how SearchFields are translated into sql query
        /// false: each search field is built with "AND"("&&") operator
        /// true: each search feild is built with "OR"( "||") operation
        /// </summary>
        public bool ConditionalOr { get; set; }
        public List<SearchField> SearchFields { get; set; }
    }
}
