using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Common.SearchModels;
using DataAccess.EntityFramework.Expressions;
using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.BD.Contact;

namespace DataAccess.EntityFramework.Repositories
{
    public interface IContactPersonRepository : IRepository<ContactPerson>
    {
        Task<SearchResult<AdminSearch>> PersonWithoutContactSearch(Search search);
        ContactPerson DeletePerson(ContactPerson person);
    }


    internal class ContactPersonRepository : Repository<ContactPerson>, IContactPersonRepository
    {
        internal ContactPersonRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<SearchResult<AdminSearch>> PersonWithoutContactSearch(Search search)
        {
            //Project & distinct & Filter
            var query = ProjectFilters(search.SearchFields, DbSet.AsQueryable())
                .SearchByFields(search.SearchFields)
                .Distinct();

            //order
            query = !string.IsNullOrEmpty(search.SortColumn) ?
                query.OrderByField(search.SortColumn, search.Order != "desc") :
                query.OrderByField("key", true);

            //both count() and skip() will trigger database query
            //TODO: may need to evaluate whether its worthwhile to do in-memory count
            return new SearchResult<AdminSearch>
            {
                Total = query.Count(),
                List =await query.Skip((search.CurrentPage - 1) * search.PageSize)
                    .Take(search.PageSize).ToListAsync()
            };
        }


        public ContactPerson DeletePerson(ContactPerson person)
        {
            var personHistoryRepo = new Repository<ContactPersonHistory>(DataContext);
            personHistoryRepo.RemoveRange(personHistoryRepo.Get(l => l.OriginalContactPersonId == person.Id).ToArray());
            Delete(person.Id);
            return person;
        }

        private IQueryable<AdminSearch> ProjectFilters(List<SearchField> searchFields, IQueryable<ContactPerson> query)
        {
            List<string> fields = searchFields.Select(l => l.Field).ToList();
            IQueryable<AdminSearch> result = query.Where(l=>l.Contacts.Count==0).Select(l => new AdminSearch
            {
                //Contact Person
                //TODO: hard code business type to 'other'
                BusinessType = "other",
                ContactPersonId = fields.Contains("ContactPersonId") ? l.Id : (int?)null,
                Key = fields.Contains("Key") ? l.Site.Key : null,
                SalesRep = null,
                Company = fields.Contains("Company") ? l.Site.Name : null,
                FirstName = fields.Contains("FirstName")?l.Firstname : null,
                LastName = fields.Contains("LastName")? l.Lastname: null,
                Position = fields.Contains("Position")?l.Position : null,
                Mobile = fields.Contains("Mobile")?l.Mobile : null,
                DirLine = fields.Contains("DirLine")?l.DirectLine : null,
                Email = fields.Contains("Email")?l.Email : null,
                NextCall =null,
                LastCall = null
            });
            return result;
        }
    }
}