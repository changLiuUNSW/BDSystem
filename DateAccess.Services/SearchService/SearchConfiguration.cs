using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccess.Common.SearchModels;
using DateAccess.Services.SearchService.Projector;

namespace DateAccess.Services.SearchService
{
    public enum SearchSortingOrder
    {
        Ascending = 0,
        Descending = 1,
    }

    public class SearchConfiguration<T> where T : class
    {
        public SearchConfiguration(Search param)
        {
            SearchParams = param;
            Predicates = new List<Expression<Func<T, bool>>>();
        }

        public Search SearchParams { get; set; } 
        public IList<Expression<Func<T, bool>>> Predicates { get; set; }
        public IQueryProjector Projector { get; set; }

        public void SetSortingOrderIfNotExist(string column, SearchSortingOrder sortingOrder)
        {
            if (SearchParams == null)
                SearchParams = new Search();

            if (string.IsNullOrEmpty(SearchParams.SortColumn))
            {
                var order = Convert.ToBoolean((int) sortingOrder);
                SearchParams.SortColumn = column;
                SearchParams.Order = order ? "desc" : "ascend";
            }
        }
    }
}
