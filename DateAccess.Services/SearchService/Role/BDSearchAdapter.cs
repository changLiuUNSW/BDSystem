using System;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Site;

namespace DateAccess.Services.SearchService.Role
{
    /// <summary>
    /// BD specific search configuration
    /// </summary>
    public class BDSearchAdapter : ISearchAdapter
    {
        public BDSearchAdapter(string initial)
        {
            if (string.IsNullOrEmpty(initial) || initial.Length < 2)
                throw new Exception("Invalid initial");

            Initial = initial;
        }

        public string BusinessType { get; set; }
        public string Initial { get; set; }

        private Expression<Func<Site, bool>> MatchCodeFromSite
        {
            get
            {
                var code = Initial.Substring(0, 2);
                
                if (string.IsNullOrEmpty(BusinessType))
                    return x => x.Contacts.Any(y => y.Code == code);

                return x => x.Contacts.Any(y => y.Code.StartsWith(code) && y.BusinessType.Type == BusinessType);
            }
        }

        private Expression<Func<Contact, bool>> MatchCodeFromContact
        {
            get
            {
                var code = Initial.Substring(0, 2);

                if (string.IsNullOrEmpty(BusinessType))
                    return x => x.Code == code;

                return x => x.Code.StartsWith(code) && x.BusinessType.Type == BusinessType;
            }
        }

        public void Configure<T>(SearchConfiguration<T> configuration) where T : class
        {
            Expression<Func<T, bool>> predicate = null;
            
            if (typeof (T) == typeof (Site))
            {
                predicate =
                    (Expression<Func<T, bool>>)
                        Convert.ChangeType(MatchCodeFromSite, typeof (Expression<Func<T, bool>>));
            }

            else if (typeof (T) == typeof (Contact))
            {
                predicate =
                    (Expression<Func<T, bool>>)
                        Convert.ChangeType(MatchCodeFromContact, typeof (Expression<Func<T, bool>>));
            }

            if (predicate != null)
                configuration.Predicates.Add(predicate);                
        }
    }
}
