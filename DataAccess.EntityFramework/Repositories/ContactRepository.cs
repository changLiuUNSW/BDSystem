using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Common.Contact;
using DataAccess.Common.SearchModels;
using DataAccess.EntityFramework.Expressions;
using DataAccess.EntityFramework.Extensions;
using DataAccess.EntityFramework.Extensions.Utilities;
using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.BD.Allocation;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DataAccess.EntityFramework.TypeLibrary;

namespace DataAccess.EntityFramework.Repositories
{
    public interface IContactRepository : IRepository<Contact>
    {
        IList<AllocationSummary> AllocationSummary();
        IList<WeeklySummary> WeeklySummary(IEnumerable<string> codes);
        IList<CompleteSummary> CompleteSummary();
        IList<AssignableSummary> AssignableSummary(IEnumerable<string> codes);
        IList<AssignableSummary> AssignableSummary();

        Contact NextCleaningContact(IEnumerable<string> qpCodes, bool isTelesale);
        Contact NextCleaningContact(int telesaleId);
        Contact NextCleaningContact(IEnumerable<string> qpCodes, int leadPersonId, IEnumerable<int> exclude = null);
        Contact NextCleaningContact(IEnumerable<string> qpCodes, IEnumerable<Allocation> allocations, IEnumerable<int> exclude);
        Task<SearchResult<AdminSearch>> Search(Search search);
    }

    internal class ContactRepository : Repository<Contact>, IContactRepository
    {
        internal ContactRepository(IDbContext dbContext) : base(dbContext)
        {
        }

        public IList<AllocationSummary> AllocationSummary()
        {
            //only OPR can be allocated
            var codes = new List<string> {"OPR"};

            var postcodeAndStates = DataContext.DbSet<Allocation>()
                .Join(DataContext.DbSet<SalesBox>(), x => x.Zone, x => x.Zone,
                    (allocation, box) => new {allocation.Size, box.Postcode, box.State, allocation.LeadPersonalId});


            var sites = DataContext.DbSet<Site>()
                .Join(postcodeAndStates, x => new {x.Size, x.Postcode, x.State}, x => new {x.Size, x.Postcode, x.State},
                    (site, a) => new {site, a.LeadPersonalId});

            return DbSet.CleaningContacts(codes)
                .ReadyToCall()
                .Join(sites, x => x.SiteId, x => x.site.Id, (contact, site) => new {contact, site}).ToList()
                .GroupBy(x => new {x.site.LeadPersonalId, x.site.site.Size, x.contact.Code, x.contact.NextCall})
                .Select(x => new AllocationSummary
                {
                    PersonId = x.Key.LeadPersonalId,
                    Code = x.Key.Code,
                    NextCall = x.Key.NextCall,
                    Inhouse = x.Count(y => y.site.site.InHouse),
                    NonInhouse = x.Count(y => y.site.site.InHouse == false),
                    Total = x.Count()
                }).ToList();
        }

        /// <summary>
        /// summary contacts that are available to call by telesales
        /// </summary>
        /// <param name="codes">contact code</param>
        /// <returns></returns>
        public IList<AssignableSummary> AssignableSummary(IEnumerable<string> codes)
        {
            var contacts = DbSet.CleaningContacts(codes).ReadyToCall();

            return contacts.GroupBy(x => new {x.Code, x.Site.State})
                .Select(x => new AssignableSummary
                {
                    Code = x.Key.Code,
                    Area = x.Key.State,
                    Inhouse = x.Count(y => y.Site.InHouse),
                    NonInhouse = x.Count(y => y.Site.InHouse == false),
                    Total = x.Count()
                }).ToList();
        }

        public IList<AssignableSummary> AssignableSummary()
        {
            var sitesWithSalesbox = DataContext.DbSet<Site>()
                .Join(DataContext.DbSet<SalesBox>(),
                    x => new {x.Postcode, x.State},
                    x => new {x.Postcode, x.State},
                    (site, salesbox) => new {site, salesbox});

            var sitesWithAllocation = sitesWithSalesbox.Join(DataContext.DbSet<Allocation>(),
                x => new {x.site.Size, x.salesbox.Zone},
                x => new {x.Size, x.Zone}, (site, allocation) => new {site.site, allocation});

            return sitesWithAllocation.Join(DbSet.CleaningContacts(new[] {"OPR"}).ReadyToCall(),
                x => x.site.Id,
                x => x.SiteId, (site, contact) => new
                {
                    site,
                    contact
                })
                .ToList()
                .GroupBy(x => new {x.contact.Code, x.site.allocation.Zone, x.site.allocation.Initial, x.site.site.Size})
                .Select(x => new AssignableSummary
                {
                    Code = x.Key.Code,
                    Area = x.Key.Zone,
                    LeadPerson = x.Key.Initial,
                    Size = x.Key.Size,
                    Inhouse = x.Count(y => y.contact.Site.InHouse),
                    NonInhouse = x.Count(y => y.contact.Site.InHouse == false),
                    Total = x.Count()
                }).ToList();
        }

        public IList<CompleteSummary> CompleteSummary()
        {
            return DbSet.CleaningContacts()
                    //.ReadyToCall()
                    .GroupBy(x => new {x.Code, x.CallFrequency, x.Site.Size})
                    .Select(x => new CompleteSummary
                    {
                        Code = x.Key.Code,
                        CallFreq = x.Key.CallFrequency,
                        Size = x.Key.Size,
                        DaToCheck = x.Count(y=>y.DaToCheck),
                        NoName = x.Count(y=>y.ReceptionName),
                        ExtManagement = x.Count(y=>y.ExtManagement),
                        Inhouse = x.Count(y=>y.Site.InHouse),
                        NonInhouse = x.Count(y=>y.Site.InHouse == false),
                        Total = x.Count()
                    }).ToList();
        }

        public IList<WeeklySummary> WeeklySummary(IEnumerable<string> codes)
        {
            var monday = DateExtension.Monday();

            var result = DbSet.CleaningContacts(codes)
                .GroupBy(x => new {x.Code, x.Site.SalesBox.Zone, x.Site.Size})
                .Select(x => new WeeklySummary
                {
                    Code = x.Key.Code,
                    Zone = x.Key.Zone,
                    Size = x.Key.Size,
                    Total = x.Count(),
                    Overdue = x.Count(y => y.NextCall < monday),
                    OverdueAndReady =
                        x.Count(
                            y =>
                                y.NextCall != null &&
                                y.NextCall < monday &&
                                y.Leads.All(lead => lead.LeadStatusId == (int) LeadStatusTypes.Cancelled) &&
                                y.Site.TsToCall)
                }).ToList();

            return result;
        }

        public Contact NextCleaningContact(IEnumerable<string> qpCodes, IEnumerable<Allocation> allocations,
            IEnumerable<int> exclude)
        {
            return
                DbSet.CleaningContacts(qpCodes)
                    .ReadyToCall()
                    .Where(x => !exclude.Contains(x.Id))
                    .FilterByAllocation(allocations)
                    .ArrangeCallOrder()
                    .FirstOrDefault();
        }

        /// <summary>
        /// return available cleaning contact for targetted lead person id base on their allocated zones
        /// </summary>
        /// <param name="qpCodes"></param>
        /// <param name="leadPersonId"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        public Contact NextCleaningContact(IEnumerable<string> qpCodes, int leadPersonId, IEnumerable<int> exclude = null)
        {
            var cleaningContacts = exclude != null
                ? DbSet.Where(x => !exclude.Contains(x.Id)).CleaningContacts(qpCodes).ReadyToCall()
                : DbSet.CleaningContacts(qpCodes).ReadyToCall();

            var allocations = DataContext.DbSet<Allocation>().Where(x => x.LeadPersonalId == leadPersonId);
            var contacts = cleaningContacts.Join(allocations, x => new {x.Site.Size, x.Site.SalesBox.Zone},
                x => new {x.Size, x.Zone},
                (contact, allocation) => contact);

            return contacts.ArrangeCallOrder().FirstOrDefault();
        }

        /// <summary>
        /// <para>return next available contact base on the allocations and qp code</para>
        /// </summary>
        /// <param name="allocations"></param>
        /// <param name="qpCodes"></param>
        /// <returns></returns>
        public Contact NextCleaningContact(IEnumerable<string> qpCodes, IEnumerable<Allocation> allocations)
        {
            return DbSet
                .CleaningContacts(qpCodes)
                .ReadyToCall()
                .FilterByAllocation(allocations)
                .ArrangeCallOrder()
                .FirstOrDefault();
        }

        /// <summary>
        /// return next available contact base on the qp codes only
        /// </summary>
        /// <param name="qpCodes"></param>
        /// <param name="isTelesale"></param>
        /// <returns></returns>
        public Contact NextCleaningContact(IEnumerable<string> qpCodes, bool isTelesale)
        {
            return DbSet
                .CleaningContacts(qpCodes)
                .ReadyToCall(isTelesale)
                .ArrangeCallOrder()
                .FirstOrDefault();
        }


        /// <summary>
        /// return next cleaning contact from telesale assignments
        /// </summary>
        /// <param name="telesaleId">telesale db key</param>
        /// <returns></returns>
        public Contact NextCleaningContact(int telesaleId)
        {
            var telesales =
                DataContext.DbSet<Telesale>()
                    .Where(x => x.Id == telesaleId)
                    .Join(DataContext.DbSet<Assignment>(), x => x.Id, x => x.TelesaleId,
                        (ts, assignment) => assignment);

            var contacts = DbSet.CleaningContacts().ReadyToCall();
            var nextContact = telesales.Join(contacts, x => new {code = x.QpCode, area = x.Area},
                x => new {code = x.Code, area = x.Site.State}, (assignment, contact) => contact)
                .ArrangeCallOrder()
                .FirstOrDefault() 
                ?? 
                telesales.Join(contacts,
                    x => new {code = x.QpCode, area = x.Area, size = x.Size},
                    x => new {code = x.Code, area = x.Site.SalesBox.Zone, size = x.Site.Size},
                    (assignment, contact) => contact)
                    .ArrangeCallOrder()
                    .FirstOrDefault();

            return nextContact;
        }

        public async Task<SearchResult<AdminSearch>> Search(Search search)
        {
            //Project & distinct & Filter    
            var query = ProjectFilters(search.SearchFields,DbSet.AsQueryable())
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
                List =await query.Skip((search.CurrentPage - 1)*search.PageSize)
                    .Take(search.PageSize).ToListAsync()
            };
        }

        private IQueryable<AdminSearch> ProjectFilters(IList<SearchField> searchFields,IQueryable<Contact> query)
        {
            var fields = searchFields.Select(l => l.Field).ToList();
            query = query.Where(l => l.ContactPersonId != null);
            var result = query.Select(l => new AdminSearch
            {
                BusinessType = fields.Contains("BusinessType")?l.BusinessType.Type:null,
                ContactPersonId = fields.Contains("ContactPersonId") ? l.ContactPersonId : null,
                Key = fields.Contains("Key")?l.Site.Key:null,
                SalesRep = fields.Contains("SalesRep") ? l.Code : null,
                Company = fields.Contains("Company") ? l.Site.Name : null,
                FirstName = fields.Contains("FirstName") && l.ContactPersonId != null ? l.ContactPerson.Firstname : null,
                LastName = fields.Contains("LastName") && l.ContactPersonId != null ? l.ContactPerson.Lastname : null,
                Position = fields.Contains("Position") && l.ContactPersonId != null ? l.ContactPerson.Position : null,
                Mobile = fields.Contains("Mobile") && l.ContactPersonId != null ? l.ContactPerson.Mobile : null,
                DirLine = fields.Contains("DirLine") && l.ContactPersonId != null ? l.ContactPerson.DirectLine : null,
                Email = fields.Contains("Email") && l.ContactPersonId != null ? l.ContactPerson.Email : null,
                NextCall = fields.Contains("NextCall") ? l.NextCall : null,
                LastCall = fields.Contains("LastCall") ? l.LastCall : null
            });
            return result;
        }
    }
}