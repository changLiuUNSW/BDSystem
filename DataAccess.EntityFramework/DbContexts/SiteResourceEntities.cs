using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
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
using DataAccess.EntityFramework.Models.Quote.Cost.Area;
using DataAccess.EntityFramework.Models.Quote.Cost.Equipment;
using DataAccess.EntityFramework.Models.Quote.Cost.Labour;
using DataAccess.EntityFramework.Models.Quote.Cost.Periodical;
using DataAccess.EntityFramework.Models.Quote.Cost.Supply;
using DataAccess.EntityFramework.Models.Quote.Cost.Vacation;
using DataAccess.EntityFramework.Models.Quote.Specification;

namespace DataAccess.EntityFramework.DbContexts
{
    internal class SiteResourceEntities : DbContext, IDbContext
    {
        public SiteResourceEntities()
            : base("Default")
        {
            Database.CreateIfNotExists();
        }

        //site tables
        //merge address table into sites
        public virtual IDbSet<Site> Sites { get; set; }
        public virtual IDbSet<SiteGroup> SiteGroups { get; set; } 
        public virtual IDbSet<CleaningContract> CleaningDetails { get; set; }
        public virtual IDbSet<SecurityContract> SecurityDetails { get; set; }
        public virtual IDbSet<BuildingType> BuildingTypes { get; set; }
        public virtual IDbSet<BuildingQualifyCriteria> BuildingQualifyCriterias { get; set; } 

        //contact tables
        public virtual IDbSet<Contact> Contacts { get; set; }
        public virtual IDbSet<ContactPerson> ContactPersons { get; set; }
        public virtual IDbSet<ContactPersonHistory> ContactPersonHistories { get; set; }
        public virtual IDbSet<BusinessType> BusinessTypes { get; set; }
        public virtual IDbSet<WeeklyReport> WeeklyReports { get; set; }
        public virtual IDbSet<FullReport> FullReports { get; set; } 

        //lead tables
        public virtual IDbSet<Lead> Leads { get; set; }
        public virtual IDbSet<LeadStatus> LeadStatus { get; set; }
        public virtual IDbSet<LeadPriority> LeadPriorities { get; set; }
        public virtual IDbSet<LeadPersonal> LeadPersonals { get; set; }
        public virtual IDbSet<LeadGroup> LeadGroups { get; set; }
        public virtual IDbSet<LeadShiftInfo> LeadShiftInfos { get; set; }
        public virtual IDbSet<OccupiedContact> OccupiedContacts { get; set; }
        public virtual IDbSet<LeadHistory> LeadHistories { get; set; } 

        //Sales box tables
        public virtual IDbSet<SalesBox> SalesBoxs { get; set; }
        public virtual IDbSet<Allocation> Allocations { get; set; }

        //telesale table
        public virtual IDbSet<Telesale> Telesales { get; set; }


        //Quote tables
        public virtual IDbSet<Quote> Quotes { get; set; }
        public virtual IDbSet<QuoteStatus> QuoteStatus { get; set; }
        public virtual IDbSet<WpRequiredInfo> WpRequiredInfos { get; set; }
        public virtual IDbSet<QuoteIssue> QuoteIssues { get; set; }
        public virtual IDbSet<QuoteHistory> QuoteHistories { get; set; }
        public virtual IDbSet<QuoteQuestion> QuoteQuestions { get; set; }
        public virtual IDbSet<QuoteAnswer> QuoteAnswers { get; set; }
        public virtual IDbSet<QuoteQuestionResult> QuoteQuestionResults { get; set; }

        //Cost tables
        //area
        public virtual IDbSet<CleaningArea> CleaningAreas { get; set; } 
        //equipment 
        public virtual IDbSet<Equipment> Equipments { get; set; } 
        public virtual IDbSet<EquipmentSupply> EquipmentSupplies { get; set; }
        public virtual IDbSet<Machine> Machines { get; set; } 
        //Periodical
        public virtual IDbSet<Periodical> Periodicals { get; set; }
        public virtual IDbSet<ExtraPeriodical> ExtraPeriodicals { get; set; }
        //Supply
        public virtual IDbSet<ToiletrySupply> ToiletrySupplies { get; set; }
        public virtual IDbSet<ExtraToiletrySupply> ExtraToiletrySupplies { get; set; }
        public virtual IDbSet<ToiletRequisite> ToiletRequisites { get; set; }
        //Vacation
        public virtual IDbSet<VacationCleaning> VacationCleanings { get; set; }
        //labour
        public virtual IDbSet<Labour> Labours { get; set; }
        public virtual IDbSet<LabourEstimation> LabourEstimations { get; set; } 
        public virtual IDbSet<LabourPeriodical> LabourPeriodicals { get; set; }
        public virtual IDbSet<AllowanceRate> AllowanceRates { get; set; }
        public virtual IDbSet<LabourRate> LabourRates { get; set; }
        public virtual IDbSet<OnCostRate> OnCostRates { get; set; } 
        //liability
        public virtual IDbSet<PublicLiability> PublicLiabilities { get; set; } 

        //specs
        public virtual IDbSet<CleaningSpec> CleaningSpecs { get; set; }
        public virtual IDbSet<SpecItem> SpecItems { get; set; }
        public virtual IDbSet<SpecTitle> SpecTitles { get; set; }

        //Quad person

        public virtual IDbSet<QuadPhoneBook> QuadPhoneBook { get; set; } 
        
       
       

        public virtual IDbSet<T> DbSet<T>() where T:class
        {
            return Set<T>();
        }

        public int Save()
        {
            return SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return SaveChangesAsync();
        }

        public DbEntityEntry DbEntry<T>(T entity) where T:class
        {
            return Entry(entity);
        }

        public void DbDispose()
        {
            Dispose();
        }

        public bool ValidConnection()
        {
            Database.Connection.Open();
            return Database.Connection.State == ConnectionState.Open;
        }

        public void EnableProxyCreation(bool set)
        {
            Configuration.ProxyCreationEnabled = set;
        }

        public void EnableLazyLoading(bool set)
        {
            Configuration.LazyLoadingEnabled = set;
        }

        public DbContextTransaction BeginTransaction(IsolationLevel isolation)
        {
            return Database.BeginTransaction(isolation);
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<decimal>().Configure(c=>c.HasPrecision(18,4));

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            /*modelBuilder.Entity<Contact>().HasRequired(l => l.BusinessType).WithMany(l => l.Contacts).WillCascadeOnDelete(false);
            modelBuilder.Entity<Lead>().HasRequired(l=>l.BusinessType).WithMany(l=>l.Leads).WillCascadeOnDelete(false);
            modelBuilder.Entity<Quote>().HasRequired(l => l.BusinessType).WithMany(l => l.Quotes).WillCascadeOnDelete(false);*/



            //selectively enable on cacade
            //modelBuilder.Entity<CleaningContract>().HasRequired(x => x.Site).WithRequiredPrincipal(x => x.CleaningContract).WillCascadeOnDelete(true);
            //modelBuilder.Entity<SecurityContract>().HasRequired(x => x.Site).WithRequiredPrincipal(x => x.SecurityContract).WillCascadeOnDelete(true);

            modelBuilder.Entity<WeeklyReport>()
                .HasMany(x => x.Details)
                .WithRequired()
                .HasForeignKey(x => x.ReportId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<FullReport>()
                .HasMany(x=>x.Details)
                .WithRequired()
                .HasForeignKey(x=>x.ReportId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Site>()
                .HasMany(x => x.Groups)
                .WithMany(x => x.Sites)
                .Map(x => x.ToTable("SiteToGroupMapping", "BD"));
        }
    }
}