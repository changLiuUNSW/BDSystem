using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Threading.Tasks;
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
using DataAccess.EntityFramework.Models.Quote.Specification;

namespace DataAccess.EntityFramework.DbContexts
{
    internal class SiteResourceEntities : DbContext
    {
        public SiteResourceEntities()
            : base("Default")
        {
            Database.CreateIfNotExists();
        }

        //site tables
        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<TempSite> TempSites { get; set; } 
        public virtual DbSet<SiteGroup> SiteGroups { get; set; }
        public virtual DbSet<ExternalManager> ExternalManagers { get; set; } 
        public virtual DbSet<CleaningContract> CleaningDetails { get; set; }
        public virtual DbSet<SecurityContract> SecurityDetails { get; set; }
        public virtual DbSet<BuildingType> BuildingTypes { get; set; }
        public virtual DbSet<BuildingQualifyCriteria> BuildingQualifyCriterias { get; set; } 

        //contact tables
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<ContactPerson> ContactPersons { get; set; }
        public virtual DbSet<ContactPersonHistory> ContactPersonHistories { get; set; }
        public virtual DbSet<BusinessType> BusinessTypes { get; set; }
        public virtual DbSet<WeeklyReport> WeeklyReports { get; set; }
        public virtual DbSet<FullReport> FullReports { get; set; } 

        //lead tables
        public virtual DbSet<Lead> Leads { get; set; }
        public virtual DbSet<LeadStatus> LeadStatus { get; set; }
        public virtual DbSet<LeadPriority> LeadPriorities { get; set; }
        public virtual DbSet<LeadPersonal> LeadPersonals { get; set; }
        public virtual DbSet<LeadGroup> LeadGroups { get; set; }
        public virtual DbSet<LeadShiftInfo> LeadShiftInfos { get; set; }
        public virtual DbSet<OccupiedContact> OccupiedContacts { get; set; }
        public virtual DbSet<LeadHistory> LeadHistories { get; set; } 
        //Sales box tables
        public virtual DbSet<SalesBox> SalesBoxs { get; set; }
        public virtual DbSet<Allocation> Allocations { get; set; }
        //telesale table
        public virtual DbSet<Telesale> Telesales { get; set; }


        //Quote tables
        public virtual DbSet<Quote> Quotes { get; set; }
        public virtual DbSet<QuoteStatus> QuoteStatus { get; set; }
        public virtual DbSet<WpRequiredInfo> WpRequiredInfos { get; set; }
        public virtual DbSet<QuoteIssue> QuoteIssues { get; set; }
        public virtual DbSet<QuoteHistory> QuoteHistories { get; set; }
        public virtual DbSet<QuoteQuestion> QuoteQuestions { get; set; }
        public virtual DbSet<QuoteAnswer> QuoteAnswers { get; set; }
        public virtual DbSet<QuoteQuestionResult> QuoteQuestionResults { get; set; }

        //Cost tables
        //area
        public virtual DbSet<CleaningArea> CleaningAreas { get; set; } 
        //equipment 
        public virtual DbSet<Equipment> Equipments { get; set; } 
        public virtual DbSet<EquipmentSupply> EquipmentSupplies { get; set; }
        public virtual DbSet<Machine> Machines { get; set; } 
        //Periodical
        public virtual DbSet<Periodical> Periodicals { get; set; }
        public virtual DbSet<ExtraPeriodical> ExtraPeriodicals { get; set; }
        //Supply
        public virtual DbSet<ToiletrySupply> ToiletrySupplies { get; set; }
        public virtual DbSet<ExtraToiletrySupply> ExtraToiletrySupplies { get; set; }
        public virtual DbSet<ToiletRequisite> ToiletRequisites { get; set; }
        //StandardRegions
        public virtual DbSet<StandardRegion> StandardRegions { get; set; }
        //QuoteSource
        public virtual DbSet<QuoteSource> QuoteSources { get; set; } 
        //labour
        public virtual DbSet<Labour> Labours { get; set; }
        public virtual DbSet<LabourEstimation> LabourEstimations { get; set; } 
        public virtual DbSet<LabourPeriodical> LabourPeriodicals { get; set; }
        public virtual DbSet<AllowanceRate> AllowanceRates { get; set; }
        public virtual DbSet<LabourRate> LabourRates { get; set; }
        public virtual DbSet<OnCostRate> OnCostRates { get; set; } 
        //liability
        public virtual DbSet<PublicLiability> PublicLiabilities { get; set; } 

        //specs
        public virtual DbSet<CleaningSpec> CleaningSpecs { get; set; }
        public virtual DbSet<SpecItem> SpecItems { get; set; }
        public virtual DbSet<SpecTitle> SpecTitles { get; set; }

        //Quad person

        public virtual DbSet<QuadPhoneBook> QuadPhoneBook { get; set; } 

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