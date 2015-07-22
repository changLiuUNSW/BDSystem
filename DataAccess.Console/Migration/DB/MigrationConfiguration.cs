using System.Collections.Generic;
using System.Collections.ObjectModel;
using DataAccess.Console.Migration.DB.Mappers;

namespace DataAccess.Console.Migration.DB
{
    internal class MigrationConfiguration
    {
        /// <summary>
        /// Phone area code
        /// </summary>
        public IDictionary<string, string> PhoneCodes { get; set; }

        public string Database { get; set; }

        /// <summary>
        /// list of tables need to be cleared before migration
        /// </summary>
        public ICollection<string> TablesToEmpty { get; set; }

        /// <summary>
        /// list of table need to re-initializae the key
        /// </summary>
        public ICollection<string> TablesToReseed { get; set; }

        /// <summary>
        /// fuzzing match probability
        /// </summary>
        public double StringSimilarityFactor { get; set; }
        
        /// <summary>
        /// tenant key config
        /// </summary>
        public int TenantKeyNumber { get; set; }
        public string TenantKeyFormat { get; set; }

        /// <summary>
        /// return formatted tenant key
        /// </summary>
        public string TenantKey
        {
            get { return TenantKeyNumber++.ToString(TenantKeyFormat); }
        }

        public StringConverter MigrationCodeConverter { get; set; }
        public StringConverter MigrationSizeConverter { get; set; }
        public List<IMigrationMapper> MigrationMappers { get; set; } 

        public MigrationConfiguration(string database)
        {
            Database = database;
            Init();
        }

        private void Init()
        {
            //set default phone code
            PhoneCodes = new Dictionary<string, string>
            {
                { "NSW", "02"},
                {"ACT", "02"},
                {"VIC", "03"},
                {"QLD", "07"},
                {"WA", "08"},
                {"SA", "08"},
                {"Mobile", "04"}
            };

            //default percentage for fuzzing match
            StringSimilarityFactor = 0.75;

            //default tennet key format is MT follow by 6 digit begin at 1
            TenantKeyNumber = 1;
            TenantKeyFormat = "MT000000.##";

            MigrationCodeConverter = new MigrationCodeConverter();
            MigrationSizeConverter = new MigrationSizeConverter();

            InitTables();
            InitMappers();
            BuildConversionList();
        }

        private void InitTables()
        {
            TablesToEmpty = new Collection<string>
            {
                "Quote.QuestionResult",
                "Quote.Issue",
                "Quote.QuoteHistory" , 
                "Quote.WPRequiredInfo",
                "Quote.Cost",
                "Quote.Quote",
                "BD.LeadHistory",
                "BD.Lead",
                "BD.CallLine",
                "BD.ContactPersonHistory",
                "BD.CleaningContract",
                "BD.SecurityContract",
                "BD.Contact",
                "BD.ContactPerson",
                "BD.SiteToGroupMapping",
                "BD.SiteGroup",
                "BD.site"
            };

            TablesToReseed = new Collection<string>
            {
                "BD.contact",
                "BD.contactperson",
                "BD.site",
                "BD.SiteGroup",
                "BD.LeadHistory",
                "BD.Lead",
                "BD.CallLine",
                "BD.ContactPersonHistory",
                "Quote.Issue",
                "Quote.QuoteHistory",
                "Quote.Quote",
                "Quote.Cost",
                "Quote.QuestionResult"
            };
        }

        private void InitMappers()
        {
            MigrationMappers = new List<IMigrationMapper>
            {
                new SiteMapper(this),
                new SiteGroupMapper(),
                new CleaningContactMapper(this),
                new SecurityContactMapper(this),
                new TenentMapper(this)
            };
        }

        private void BuildConversionList()
        {
            MigrationCodeConverter.Build();
            MigrationSizeConverter.Build();
        }
    }
}
