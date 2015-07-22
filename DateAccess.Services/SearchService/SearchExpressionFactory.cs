using System;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.EntityFramework.Models.BD.Site;

namespace DateAccess.Services.SearchService
{
    /// <summary>
    /// store re-useable expressions for search functions
    /// </summary>
    public class SearchExpressionLibrary
    {
        public static Expression<Func<Site, bool>> MatchBusinessTypeForSite(string businessType)
        {
            return site => site.Contacts.Any(x => x.BusinessType.Type == businessType);
        }

        
    }
}
