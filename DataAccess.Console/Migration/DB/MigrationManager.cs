using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DataAccess.Console.Context;
using DataAccess.Console.Migration.Excel;
using DataAccess.Console.Models;
using DataAccess.EntityFramework.DbContexts;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services;

namespace DataAccess.Console.Migration.DB
{
    internal class MigrationManager : IDisposable
    {
        internal enum GroupNameCorrectionOption
        {
            MergeWithNameChangeToExist = 1,
            MergeWithNameChangeToNew = 2,
            CreateNewSiteGroup = 3,
            Skip = 4
        }

        public MigrationConfiguration Configuration { get; set; }
        public SiteResourceEntities Destination { get; set; }
        public CompanyContext Source { get; set; }
        private List<Func<object, bool>> Validators { get; set; } 

        public MigrationManager(CompanyContext source,
                                SiteResourceEntities destination,
                                MigrationConfiguration configuration)
        {
            Destination = destination;
            Source = source;
            Configuration = configuration;
        }

        public void Start()
        {
            Destination.Configuration.AutoDetectChangesEnabled = false;
            Destination.Configuration.ValidateOnSaveEnabled = false;
            Source.Configuration.AutoDetectChangesEnabled = false;
            Source.Configuration.ValidateOnSaveEnabled = false;

            //CleanUpDatabase();
            //CreateBusinessTypesIfNotExist();
            CopyDatabase();
        }

        private void CleanUpDatabase()
        {
            foreach (var table in Configuration.TablesToEmpty)
            {
                var tableName = Configuration.Database + "." + table;

                Destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable(tableName));
            }

            foreach (var table in Configuration.TablesToReseed)
            {
                var tableName = Configuration.Database + "." + table;
                Destination.Database.ExecuteSqlCommand(SqlCmd.Reseed(tableName, 0));
            }
        }

        private void CreateBusinessTypesIfNotExist()
        {
            if (Destination.BusinessTypes.Any())
                return;

            var businessTypes = Enum.GetValues(typeof (BusinessTypes)).Cast<int>().Select(x=>new BusinessType
            {
                Id = x,
                Type = Enum.GetName(typeof(BusinessTypes), x)
            });

            Destination.BusinessTypes.AddRange(businessTypes);
            Destination.SaveChanges();
        }

        private void CopyDatabase()
        {
            Validators = new List<Func<object, bool>>
            {
                ValidateSite,
                ValidSiteGroup,
                ValidateContact,
                ValidateTenant
            };

            var count = 0;

            //its import to return no tracking record because legacy DB imported into MSSQL does not have a valid primary key
            foreach (var row in Source.COMPTEMP.AsNoTracking())
            {
                count++;
                System.Console.WriteLine("{0} - {1}, Total : {2}", row.COMPANY, row.KEY, count);
                MappedData.Reset();
                Map(row);
                MappedData.Save(Destination);
            }
        }

        private void Map(COMPTEMP row)
        {
            foreach (var mapper in Configuration.MigrationMappers) 
            {
                var result = mapper.Map(row);
                ProcessMappingResult(result);
            }
        }

        private void ProcessMappingResult(object mapResult)
        {
            var enumerator = Validators.GetEnumerator();
            var valid = false;

            while (enumerator.MoveNext() && valid == false)
            {
                if (enumerator.Current == null)
                    continue;

                valid = enumerator.Current(mapResult);
            }
        }

        private bool ValidateSite(object obj)
        {
            var site = obj as Site;
            if (site == null) 
                return false;

            MappedData.Site = site;
            return true;
        }

        private bool ValidSiteGroup(object obj)
        {
            var siteGroup = obj as SiteGroup;
            if (siteGroup == null)
                return false;

            MappedData.SiteGroup = siteGroup;

            var similarGroups =
                Destination.SiteGroups.Local.Where(
                    x =>
                        Util.StringSimilarity.Compare(x.GroupName, MappedData.SiteGroup.GroupName) >=
                        Configuration.StringSimilarityFactor).ToList();

            if (similarGroups.Any())
                MappedData.CorrectGroupName(similarGroups);

            return true;
        }

        private bool ValidateContact(object obj)
        {
            var contact = obj as Contact;
            if (contact == null)
                return false;

            MappedData.Contacts.Add(contact);

            if (MappedData.Site != null)
            {
                if (MappedData.Site.ContactPersons == null)
                    MappedData.Site.ContactPersons = new Collection<ContactPerson>();

                MappedData.Site.ContactPersons.Add(contact.ContactPerson);
            }

            return true;
        }

        private bool ValidateTenant(object obj)
        {
            var tenants = obj as List<Site>;
            if (tenants == null)
                return false;

            if (tenants.Count > 0)
            {
                if (MappedData.Site.Groups == null || MappedData.Site.Groups.Count <= 0)
                    MappedData.Site.Groups = new Collection<SiteGroup>();

                var siteGroup = new SiteGroup
                {
                    GroupName = string.Format("{0}, {1}, {2} {3}, {4}", 
                    MappedData.Site.Number, 
                    MappedData.Site.Street, 
                    MappedData.Site.Suburb, 
                    MappedData.Site.State,
                    MappedData.Site.Postcode),
                    Type = "Building",
                    Sites = new Collection<Site>
                    {
                        MappedData.Site,
                    }
                };

                tenants.AsParallel().ForAll(x => siteGroup.Sites.Add(x));
                MappedData.Tenants = tenants;
            }

            return true;
        }

        public void Dispose()
        {
            Destination.Dispose();
            Source.Dispose();
        }

        /// <summary>
        /// new db models from the current row
        /// </summary>
        private static class MappedData
        {
            static MappedData()
            {
                Reset();
            }

            public static Site Site { get; set; }
            public static SiteGroup SiteGroup { get; set; }
            public static List<Contact> Contacts { get; set; }
            public static List<Site> Tenants { get; set; }

            public static void Reset()
            {
                Site = null;
                SiteGroup = null;
                Contacts = new List<Contact>();
                Tenants = new List<Site>();
            }

            public static void CorrectGroupName(IReadOnlyCollection<SiteGroup> groups)
            {
                if (SiteGroup == null)
                    return;

                var orderedGroups =
                    groups.OrderByDescending(x => Util.StringSimilarity.Compare(x.GroupName, SiteGroup.GroupName)).ToList();

                for (var i = 0; i < orderedGroups.Count; i ++)
                {
                    var group = orderedGroups[i];

                    var probability = Util.StringSimilarity.Compare(group.GroupName, SiteGroup.GroupName);
                    if (probability < 1)
                    {
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine(
                            string.Format("\nWARNING SIMILAR GROUP NAME FOUND {0} - {1}: PROBABILITY ", groups.Count, i + 1) +
                            probability.ToString("#0.##%"));
                        System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                        System.Console.WriteLine("Existing Group: {0}", group.GroupName);
                        System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                        System.Console.WriteLine("New Group: {0}\n", SiteGroup.GroupName);
                        System.Console.ResetColor();

                        var result = UserInput.Get<GroupNameCorrectionOption>(
                            string.Format(
                                "1. Merge group and use {0} as group name\n" +
                                "2. Merge group and use {1} as group name\n" +
                                "3. Add as a different group\n" +
                                "4. Next matching", group.GroupName, SiteGroup.GroupName));

                        var end = true;

                        switch (result)
                        {
                            case GroupNameCorrectionOption.MergeWithNameChangeToExist:
                                group.Sites.Add(Site);
                                Site.Groups.Add(group);
                                //clear the new site group
                                SiteGroup = null;
                                break;
                            case GroupNameCorrectionOption.MergeWithNameChangeToNew:
                                group.GroupName = SiteGroup.GroupName;
                                group.Sites.Add(Site);
                                Site.Groups.Add(group);
                                //clear the new site group
                                SiteGroup = null;
                                break;
                            case GroupNameCorrectionOption.CreateNewSiteGroup:
                                SiteGroup.Sites.Add(Site);
                                Site.Groups.Add(SiteGroup);
                                break;
                            case GroupNameCorrectionOption.Skip:
                                if (i < orderedGroups.Count)
                                {
                                    end = false;
                                }
                                else
                                {
                                    //if user decided to skip the last site group then no group will be created at all
                                    SiteGroup = null;
                                    end = true;
                                }
                                    
                                break;
                        }

                        if (end)
                            return;
                    }
                    else
                    {
                        //this cater for the name is 100% identical
                        group.Sites.Add(Site);
                        Site.Groups.Add(group);
                        SiteGroup = null;
                        return;
                    }
                }
            }

            public static void Save(SiteResourceEntities context)
            {
                if (Site != null)
                    context.Sites.Add(Site);

                if (SiteGroup != null)
                    context.SiteGroups.Add(SiteGroup);

                if (Contacts.Count > 0)
                    context.Contacts.AddRange(Contacts);

                if (Tenants.Count > 0)
                    context.Sites.AddRange(Tenants);
            }
        }

        private static class UserInput
        {
            public static T Get<T>(string message) where T: struct, IConvertible
            {
                System.Console.WriteLine(message);
                return GetInput<T>();
            }

            private static T GetInput<T>() where T: struct, IConvertible
            {
                var input = System.Console.ReadLine();
                T option;
                var success = Enum.TryParse(input, out option);

                if (!success || !Enum.IsDefined(typeof(T), option))
                {
                    System.Console.WriteLine("Invalid option");
                    GetInput<T>();
                }

                return option;
            }
        }
    }
}
