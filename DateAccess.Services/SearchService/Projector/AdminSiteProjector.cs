using System;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.Common.SearchModels;
using DataAccess.EntityFramework.Models.BD.Site;

namespace DateAccess.Services.SearchService.Projector
{
    /// <summary>
    /// project site table into adminsearch, default type is cleaning if no type is supplied
    /// </summary>
    public class AdminSiteProjector : IQueryProjector
    {
        private const string DefaultType = "Cleaning";
        public string BusinessType { get; set; }

        public AdminSiteProjector()
        {
            
        }

        public AdminSiteProjector(string businessType)
        {
            BusinessType = businessType;
        }

        public Expression<Func<T, TResult>> CreateExpression<T, TResult>()
            where T : class
            where TResult : class
        {
            return (Expression<Func<T, TResult>>) Convert.ChangeType(Expression, typeof (Expression<Func<T, TResult>>));
        }

        private Expression<Func<Site, AdminSearch>> Expression
        {
            get
            {
                var type = BusinessType ?? DefaultType;

                return x => new AdminSearch
                {
                    BusinessType = type,
                    SiteId = x.Id,
                    Key = x.Key,
                    SalesRepList =
                        x.Contacts.Where(y => y.BusinessType.Type == type)
                            .Select(j => j.Code)
                            .Distinct(),
                    Company = x.Name,
                    Unit = x.Unit,
                    Number = x.Number,
                    Street = x.Street,
                    Suburb = x.Suburb,
                    State = x.State,
                    PostCode = x.Postcode,
                    Phone = x.Phone
                };
            }
        }
    }
}
