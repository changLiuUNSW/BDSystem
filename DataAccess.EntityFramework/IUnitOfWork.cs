using System.Data;
using System.Data.Entity;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.BD.Allocation;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DataAccess.EntityFramework.Models.Quad;
using DataAccess.EntityFramework.Models.Quote;
using DataAccess.EntityFramework.Models.Quote.Cost;
using DataAccess.EntityFramework.Models.Quote.Cost.Equipment;
using DataAccess.EntityFramework.Models.Quote.Cost.Supply;
using DataAccess.EntityFramework.Models.Quote.Specification;
using DataAccess.EntityFramework.Repositories;


namespace DataAccess.EntityFramework
{
    public interface IUnitOfWork
    {
        IContactRepository ContactRepository { get; }
        ILeadRepository LeadRepository { get; }
        ISalesBoxRepository SalesBoxRepository { get; }
        ISiteRepository SiteRepository { get; }
        ISiteGroupRepository SiteGroupRepository { get; }
        IContactPersonRepository ContactPersonRepository { get; }
        IRepository<Allocation> AllocationRepository { get;}
        ILeadPersonRepository LeadPersonalRepository { get; }
        IQuoteRepository QuoteRepository { get; }
        IQuoteCostRepository QuoteCostRepository { get; }
        IRepository<Equipment> EquipmentRepository { get; }
        IRepository<Machine> EquipmentTypeRepository { get; }
        IRepository<ToiletRequisite> ToiletRequisiteRepository { get; }
        IRepository<Margin> QuoteMarginRepository { get; }
        IRepository<LeadPriority> PriorityGroupRepository { get; }
        IRepository<Telesale> TelesaleRepository { get; } 
        IRepository<Assignment> TelesaleAssignmentRepository { get; }
        IRepository<CallLine> CallLineRepository { get; }
        IRepository<ContactPersonHistory> ContactPersonHistoryRepository { get; }
        IRepository<CleaningSpec> CleaningSpecRepository { get; }
        IRepository<SpecItem> SpecItemRepository { get; }
        IRepository<LeadShiftInfo> LeadShiftInfoRepository { get; }
        IRepository<LeadGroup> LeadGroupRepository { get; }
        IRepository<OccupiedContact> OccupiedContactRepository { get; }
        IRepository<WeeklyReport> WeeklyReportRepository { get; }
        IRepository<FullReport> FullReportRepository { get; }
        IRepository<QuadPhoneBook> PhoneBookRepository { get; }
        IRepository<T> GetRepository<T>() where T : class;
        DbContextTransaction BeginTransaction(IsolationLevel isolation);
        void EnableProxyCreation(bool set);
        int Save();
        Task<int> SaveAsync();
    }
}
