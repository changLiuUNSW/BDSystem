using System;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.BD.Allocation;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DataAccess.EntityFramework.Models.Quad;
using DataAccess.EntityFramework.Models.Quote.Cost;
using DataAccess.EntityFramework.Models.Quote.Cost.Equipment;
using DataAccess.EntityFramework.Models.Quote.Cost.Supply;
using DataAccess.EntityFramework.Models.Quote.Specification;
using DataAccess.EntityFramework.Repositories;

namespace DataAccess.EntityFramework
{
    internal class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DbContext _context;
        private bool _disposed;
        private IRepository<Allocation> _allocationRepository;
        private IRepository<CallLine> _callLineRepository;
        private IRepository<ContactPersonHistory> _contactPersonHistoryRepository;
        private IContactPersonRepository _contactPersonRepository;
        private IContactRepository _contactRepository;
        private IRepository<Equipment> _equipmentRepository;
        private IRepository<Machine> _equipmentTypeRepository;
        private IRepository<LeadGroup> _leadGroupRepository;
        private ILeadPersonRepository _leadPersonalRepository;
        private ILeadRepository _leadRepository;
        private IQuoteCostRepository _quoteCostRepository;
        private IRepository<LeadShiftInfo> _leadShiftInfoRepository;
        private IRepository<LeadPriority> _priorityGroupRepository;
        private IRepository<Margin> _quoteMarginRepository;
        private IQuoteRepository _quoteRepository;
        private ISalesBoxRepository _salesBoxRepository;
        private ISiteRepository _siteRepository;
        private ITempSiteRepository _tempSiteRepository;
        private ISiteGroupRepository _siteGroupRepository;
        private IRepository<CleaningSpec> _cleaningSpecRepository; 
        private IRepository<SpecItem> _specItemRepository;
        private IRepository<Telesale> _telesaleRepository; 
        private IRepository<Assignment> _telesaleAssignmentRepository;
        private IRepository<ToiletRequisite> _toiletRequisiteRepository;
        private IRepository<OccupiedContact> _occupiedContactRepository;
        private IRepository<WeeklyReport> _weeklyReportRepository;
        private IRepository<FullReport> _fullReportRepository;
        private IRepository<QuadPhoneBook> _phoneBookRepository;
        private IRepository<ExternalManager> _extManagerRepository; 

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     public implementation of dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //----------BEGIN--------
        //lazy loading repository entities
        public IContactRepository ContactRepository
        {
            get { return _contactRepository ?? (_contactRepository = new ContactRepository(_context)); }
        }

        public ILeadRepository LeadRepository
        {
            get { return _leadRepository ?? (_leadRepository = new LeadRepository(_context)); }
        }

        public IRepository<Allocation> AllocationRepository
        {
            get { return _allocationRepository ?? (_allocationRepository = new Repository<Allocation>(_context)); }
        }

        public IQuoteRepository QuoteRepository
        {
            get { return _quoteRepository ?? (_quoteRepository = new QuoteRepository(_context)); }
        }

        public IQuoteCostRepository QuoteCostRepository
        {
            get { return _quoteCostRepository ?? (_quoteCostRepository = new QuoteCostRepository(_context)); }
        }

        public IRepository<Equipment> EquipmentRepository
        {
            get { return _equipmentRepository ?? (_equipmentRepository = new Repository<Equipment>(_context)); }
        }

        public IRepository<Machine> EquipmentTypeRepository
        {
            get
            {
                return _equipmentTypeRepository ?? (_equipmentTypeRepository = new Repository<Machine>(_context));
            }
        }

        public IRepository<ToiletRequisite> ToiletRequisiteRepository
        {
            get
            {
                return _toiletRequisiteRepository ??
                       (_toiletRequisiteRepository = new Repository<ToiletRequisite>(_context));
            }
        }

        public IRepository<Margin> QuoteMarginRepository
        {
            get { return _quoteMarginRepository ?? (_quoteMarginRepository = new Repository<Margin>(_context)); }
        }

        public ILeadPersonRepository LeadPersonalRepository
        {
            get
            {
                return _leadPersonalRepository ??
                       (_leadPersonalRepository = new LeadPersonRepository(_context));
            }
        }

        public IRepository<LeadPriority> PriorityGroupRepository
        {
            get
            {
                return _priorityGroupRepository ??
                       (_priorityGroupRepository = new Repository<LeadPriority>(_context));
            }
        }

        public ISalesBoxRepository SalesBoxRepository
        {
            get { return _salesBoxRepository ?? (_salesBoxRepository = new SalesBoxRepository(_context)); }
        }

        public IRepository<Telesale> TelesaleRepository
        {
            get { return _telesaleRepository ?? (_telesaleRepository = new Repository<Telesale>(_context)); }
        }

        public IRepository<Assignment> TelesaleAssignmentRepository
        {
            get
            {
                return _telesaleAssignmentRepository ??
                       (_telesaleAssignmentRepository = new Repository<Assignment>(_context));
            }
        }

        public ITempSiteRepository TempSiteRepository { get { return _tempSiteRepository ?? (_tempSiteRepository = new TempSiteRepository(_context)); } }

        public ISiteGroupRepository SiteGroupRepository
        {
            get { return _siteGroupRepository ?? (_siteGroupRepository = new SiteGroupRepository(_context)); }
        }


        public ISiteRepository SiteRepository
        {
            get { return _siteRepository ?? (_siteRepository = new SiteRepository(_context)); }
        }

        public IRepository<CallLine> CallLineRepository
        {
            get { return _callLineRepository ?? (_callLineRepository = new Repository<CallLine>(_context)); }
        }

        public IContactPersonRepository ContactPersonRepository
        {
            get
            {
                return _contactPersonRepository ?? (_contactPersonRepository = new ContactPersonRepository(_context));
            }
        }

        public IRepository<ContactPersonHistory> ContactPersonHistoryRepository
        {
            get
            {
                return _contactPersonHistoryRepository ??
                       (_contactPersonHistoryRepository = new Repository<ContactPersonHistory>(_context));
            }
        }

        public IRepository<CleaningSpec> CleaningSpecRepository
        {
            get
            {
                return _cleaningSpecRepository ?? (_cleaningSpecRepository = new Repository<CleaningSpec>(_context));
            }
        }

        public IRepository<SpecItem> SpecItemRepository
        {
            get { return _specItemRepository ?? (_specItemRepository = new Repository<SpecItem>(_context)); }
        }

        public IRepository<LeadShiftInfo> LeadShiftInfoRepository
        {
            get
            {
                return _leadShiftInfoRepository ?? (_leadShiftInfoRepository = new Repository<LeadShiftInfo>(_context));
            }
        }

        public IRepository<LeadGroup> LeadGroupRepository
        {
            get { return _leadGroupRepository ?? (_leadGroupRepository = new Repository<LeadGroup>(_context)); }
        }

        public IRepository<OccupiedContact> OccupiedContactRepository
        {
            get
            {
                return _occupiedContactRepository ??
                       (_occupiedContactRepository = new Repository<OccupiedContact>(_context));
            }
        }

        public IRepository<WeeklyReport> WeeklyReportRepository
        {
            get
            {
                return _weeklyReportRepository ?? (_weeklyReportRepository = new Repository<WeeklyReport>(_context));
            }
        }

        public IRepository<FullReport> FullReportRepository
        {
            get
            {
                return _fullReportRepository ?? (_fullReportRepository = new Repository<FullReport>(_context));
            }
        }

        public IRepository<QuadPhoneBook> PhoneBookRepository
        {
            get { return _phoneBookRepository ?? (_phoneBookRepository = new Repository<QuadPhoneBook>(_context)); }
        }

        public IRepository<ExternalManager> ExtManagerRepository
        {
            get { return _extManagerRepository ?? (_extManagerRepository = new Repository<ExternalManager>(_context)); }
        }

        //lazy loading repository entities
        //----------END----------

        /// <summary>
        ///     save tracked changes into data context
        /// </summary>
        /// <returns></returns>
        public virtual int Save()
        {
            return _context.SaveChanges();
        }

        public virtual Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public virtual void EnableProxyCreation(bool set)
        {
            _context.Configuration.ProxyCreationEnabled = set;
        }

        public virtual IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(_context);
        }

        public DbContextTransaction BeginTransaction(IsolationLevel isolation)
        {
            return _context.Database.BeginTransaction(isolation);
        }

        /// <summary>
        ///     de-contructor
        /// </summary>
        ~UnitOfWork()
        {
            Dispose(false);
        }

        /// <summary>
        ///     db context will be injected by dependency container thus the lifetime of the db context is managed by the
        ///     dependency container
        ///     dispose any unmanaged resources other than db context here
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
            }

            _disposed = true;

            // Free any unmanaged objects here. 
        }
    }
}