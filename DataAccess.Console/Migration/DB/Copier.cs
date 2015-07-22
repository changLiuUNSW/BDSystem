using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;
using DataAccess.Console.Context;
using DataAccess.Console.Migration.Excel;
using DataAccess.Console.Models;
using DataAccess.EntityFramework.DbContexts;
using DataAccess.EntityFramework.Extensions;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services;

namespace DataAccess.Console.Migration.DB
{
    /// <summary>
    /// this class is deprecated use migration manager instead
    /// </summary>
    internal static class Migration
    {
        public static Dictionary<string, string> PhoneCodes = new Dictionary<string, string>
        {
            { "NSW", "02"},
            {"ACT", "02"},
            {"VIC", "03"},
            {"QLD", "07"},
            {"WA", "08"},
            {"SA", "08"},
            {"Mobile", "04"}
        };

        private enum ToOprList
        {
            DBL,
            DBO,
            DBT,
            ESL,
            EST,
            FRL,
            FRT,
            ISL,
            ISO,
            IST,
            JFT,
            JPL,
            JPO,
            JPT,
            LCT,
            MCT,
            OP,
            OPM,
            OPS,
            PAL,
            PAT,
            QA,
            QT,
            TBL,
            TBT
        }

        private enum To008List
        {
            OPM,
            OPS,
            QT
        }

        private enum To025List
        {
            DBO,
            ISO,
            JPO,
            OP,
            QA
        }

        private enum To050List
        {
            DBT,
            EST,
            FRT,
            IST,
            JFT,
            JPT,
            LCT,
            MCT,
            PAT,
            TBT
        }

        private enum To120List
        {
            DBL,
            ESL,
            FRL,
            ISL,
            JPL,
            PAL,
            TBL
        };

        public const double NameSimilarityFactor = 0.85;

        private const string TenantKeyFormat = "000000.##";
        private static int _tenantKey = 0;

        private static readonly IDictionary<string, string> CodeToChange;
        private static readonly IDictionary<string, string> SizeToChange;

        private static readonly IDictionary<Type, string> SizeTypes; 

        static Migration()
        {
            CodeToChange = new Dictionary<string, string>();
            SizeToChange = new Dictionary<string, string>();
            SizeTypes = new Dictionary<Type, string>();

            InitCodes();
            InitSizeTypes();
            InitSizes();
        }

        private static void InitCodes()
        {
            //init code to change list
            foreach (var name in Enum.GetNames(typeof(ToOprList)))
            {
                CodeToChange[name] = "OPR";
            }

            CodeToChange["G"] = "GRP";
        }

        private static void InitSizeTypes()
        {
            SizeTypes.Add(typeof(To008List), Size.Size008.GetDescription());
            SizeTypes.Add(typeof(To025List), Size.Size025.GetDescription());
            SizeTypes.Add(typeof(To050List), Size.Size050.GetDescription());
            SizeTypes.Add(typeof(To120List), Size.Size120.GetDescription());
        }

        private static void InitSizes()
        {
            foreach (var sizeType in SizeTypes)
            {
                foreach (var name in Enum.GetNames(sizeType.Key))
                {
                    SizeToChange[name] = sizeType.Value;
                }
            }
        }

        public static string GetCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            var upper = code.ToUpper();

            if (!CodeToChange.ContainsKey(upper))
                return code;

            return CodeToChange[upper];
        }

        public static string GetSize(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            var upper = code.ToUpper();

            if (!SizeToChange.ContainsKey(upper))
                return null;

            return SizeToChange[upper];
        }

        public static string NextTenantKey()
        {
            var key = "MT" + _tenantKey.ToString(TenantKeyFormat);
            _tenantKey++;
            return key;
        }
    }

    internal static class Copier
    {
        public static void Begin()
        {
            try
            {
                using (var destination = new SiteResourceEntities())
                using (var origin = new CompanyContext())
                {
                    destination.Configuration.AutoDetectChangesEnabled = false;
                    destination.Configuration.ValidateOnSaveEnabled = false;
                    origin.Configuration.AutoDetectChangesEnabled = false;
                    origin.Configuration.ValidateOnSaveEnabled = false;

                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.Quote.QuestionResult"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.Quote.Issue"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.Quote.QuoteHistory"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.Quote.WPRequiredInfo"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.Quote.Cost"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.Quote.Quote"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.LeadHistory"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.Lead"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.CallLine"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.ContactPersonHistory"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.CleaningContract"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.SecurityContract"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.Contact"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.ContactPerson"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.SiteToGroupMapping"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.ExternalManager"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.SiteGroup"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.site"));
                    destination.Database.ExecuteSqlCommand(SqlCmd.Reseed("ims_test.bd.contact", 0));
                    destination.Database.ExecuteSqlCommand(SqlCmd.Reseed("ims_test.bd.contactperson", 0));
                    destination.Database.ExecuteSqlCommand(SqlCmd.Reseed("ims_test.bd.site", 0));
                    destination.Database.ExecuteSqlCommand(SqlCmd.Reseed("ims_test.bd.SiteGroup", 0));
                    destination.Database.ExecuteSqlCommand(SqlCmd.Reseed("IMS_Test.bd.LeadHistory", 0));
                    destination.Database.ExecuteSqlCommand(SqlCmd.Reseed("IMS_Test.bd.Lead", 0));
                    destination.Database.ExecuteSqlCommand(SqlCmd.Reseed("IMS_Test.bd.CallLine", 0));
                    destination.Database.ExecuteSqlCommand(SqlCmd.Reseed("IMS_Test.bd.ContactPersonHistory", 0));
                    destination.Database.ExecuteSqlCommand(SqlCmd.Reseed("IMS_Test.Quote.Issue", 0));
                    destination.Database.ExecuteSqlCommand(SqlCmd.Reseed("IMS_Test.Quote.QuoteHistory", 0));
                    destination.Database.ExecuteSqlCommand(SqlCmd.Reseed("IMS_Test.Quote.Quote", 0));
                    destination.Database.ExecuteSqlCommand(SqlCmd.Reseed("IMS_Test.Quote.Cost", 0));
                    destination.Database.ExecuteSqlCommand(SqlCmd.Reseed("IMS_Test.Quote.QuestionResult", 0));
                    


                    //first to create business types
                    if (!destination.BusinessTypes.Any())
                    {
                        foreach (var businessType in GetBusinessTypes())
                        {
                            destination.BusinessTypes.Add(businessType);
                        }

                        destination.SaveChanges();
                    }

                    foreach (var row in origin.COMPTEMP.ToList())
                    {
                        var site = CopySite(row);

                        var siteGroup = CopySiteGroup(row);

                        if (siteGroup != null)
                        {
                            //var group = destination.SiteGroups.Local.SingleOrDefault(x => x.Name.ToUpper() == siteGroup.Name.ToUpper());
                            var group =
                                destination.SiteGroups.Local.SingleOrDefault(
                                    x => Util.StringSimilarity.Compare(x.GroupName, siteGroup.GroupName) >= Migration.NameSimilarityFactor);
                            if (group != null)
                            {
                                if (group.Sites == null)
                                    group.Sites = new Collection<Site>();
                                group.Sites.Add(site);

                                if (site.Groups == null)
                                    site.Groups = new Collection<SiteGroup>();
                                site.Groups.Add(group);
                            }
                            else
                            {
                                if (siteGroup.Sites == null)
                                    siteGroup.Sites = new Collection<Site>();
                                siteGroup.Sites.Add(site);

                                if(site.Groups == null)
                                    site.Groups = new Collection<SiteGroup>();
                                site.Groups.Add(siteGroup);
                                
                                destination.SiteGroups.Add(siteGroup);
                            }
                        }

                        if (row.BUILD_TYPE != null)
                        {
                            var buildingType = destination.BuildingTypes.SingleOrDefault(x => x.Code == row.BUILD_TYPE);
                            if (buildingType != null)
                                site.BuildTypeId = buildingType.Id;
                        }

                        site.Contacts = CopyContacts(row);
                        site.CleaningContract = CopyCleaningContract(row);
                        site.SecurityContract = CopySecurityContract(row);

                        if (site.Contacts != null)
                            site.ContactPersons = site.Contacts.Select(x => x.ContactPerson).ToList();

                        if (row.SALES_REP == "PMS")
                        {
                            var tenants = CopyTenant(row);

                            if (tenants.Count > 0)
                            {
                                if (site.Groups == null || site.Groups.Count <= 0)
                                    site.Groups = new Collection<SiteGroup>();
                                
                                site.Groups.Add(new SiteGroup
                                {
                                    GroupName = string.Format("{0} {1}, {2} {3}, {4}", 
                                    site.Number, site.Street, site.Suburb, site.State, site.Postcode),
                                    Type = "Building",
                                    Sites = new Collection<Site>()
                                });

                                foreach (var group in site.Groups)
                                {
                                    foreach (var tenant in tenants)
                                    {
                                        group.Sites.Add(tenant);

                                        if (tenant.Groups == null)
                                            tenant.Groups = new Collection<SiteGroup>();

                                        tenant.Groups.Add(group);
                                        destination.Sites.Add(tenant);
                                    }
                                }
                            }
                        }

                        destination.Sites.Add(site);
                        System.Console.WriteLine("Site: " + row.KEY + " " + row.COMPANY);
                    }

                    System.Console.WriteLine("Saving into new database, please wait");
                    destination.SaveChanges();
                    System.Console.WriteLine("extracting finished, press any keys to continue");
                    System.Console.ReadKey();
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var item in ex.EntityValidationErrors)
                {
                }
            }
            catch (DbUpdateException ex)
            {
                foreach (var item in ex.Entries)
                {
                }
            }
            catch (Exception ex)
            {
                var i = ex;
            }
        }



        private static IEnumerable<BusinessType> GetBusinessTypes()
        {
            foreach (var value in Enum.GetValues(typeof(BusinessTypes)))
            {
                yield return new BusinessType
                {
                    Id = Convert.ToInt32(value),
                    Type = Enum.GetName(typeof(BusinessTypes), value)
                };
            }
        }

        private static Site CopySite(COMPTEMP origin)
        {
            return new Site
            {
                Key = origin.KEY,
                Unit = origin.UNIT,
                Number = origin.NUMBER,
                Street = origin.STREET,
                Suburb = origin.SUBURB,
                State = origin.STATE,
                Postcode = origin.P_CODE,
                Name = origin.COMPANY,
                BuildingName = origin.BUILD_ID,
                //PropertyManagerName = origin.AGENT_COMP,
                PropertyManaged = origin.MANBYAGENT,
                PMSite = origin.PM_SITE,
                Phone = origin.PHONE,
                InHouse = origin.IN_HOUSE,
                Size = Migration.GetSize(origin.SALES_REP),
                TsToCall = origin.TSTOCALL,
                Qualification = origin.QUALI_NO
            };
        }

        private static IList<Site> CopyTenant(COMPTEMP origin)
        {
            var list = new List<Site>();
            var phoneCode = Migration.PhoneCodes[origin.STATE];

            if (!string.IsNullOrEmpty(origin.M_TENANT1))
            {
                list.Add(CopyTenantInfo(origin, origin.M_TENANT1));
            }

            if (!string.IsNullOrEmpty(origin.M_TENANT2))
            {
                list.Add(CopyTenantInfo(origin, origin.M_TENANT2));
            }

            return list;
        }

        private static Site CopyTenantInfo(COMPTEMP origin, string tenant)
        {
            var phoneCode = Migration.PhoneCodes[origin.STATE];

            var site = new Site
            {
                Key = Migration.NextTenantKey(),
                Number = origin.NUMBER,
                Street = origin.STREET,
                Suburb = origin.SUBURB,
                State = origin.STATE,
                Postcode = origin.P_CODE,
                BuildingName = origin.BUILD_ID,
                PropertyManaged = true,
            };

            var regex = string.Format(@"\(?{0}\)?\s*\d{{4,8}}?\s*\d{{4,8}}?", phoneCode);
            if (!MatchTenantWithRegex(site, tenant, regex))
            {
                regex = string.Format(@"{0}\d{{2,2}}\s?\d{{3,3}}\s?\d{{3,3}}", Migration.PhoneCodes["Mobile"]);
                MatchTenantWithRegex(site, tenant, regex);
            }

            return site;
        }

        private static bool MatchTenantWithRegex(Site site, string target, string regex)
        {
            var regexMatch = Regex.Match(target, regex, RegexOptions.IgnoreCase);

            if (regexMatch.Success)
            {
                var matched = regexMatch.Groups[regexMatch.Groups.Count - 1].Value;
                if (matched.Length > 15)
                    site.Phone = matched.Substring(0, 15);
                else
                    site.Phone = matched;

                site.Name = Regex.Replace(target, regex, "");
            }
            else
            {
                site.Name = target;
            }

            return regexMatch.Success;
        }

        private static IList<Contact> CopyContacts(COMPTEMP origin)
        {
            var contacts = new List<Contact>();

            if (!origin.CLEAN_CONT /*&& !origin.MAINT_CONT*/ && !origin.SECU_CONT)
                return null;

            if (origin.CLEAN_CONT)
            {
                var contact = CopyCleaningContact(origin);
                CopyContact(origin, ref contact);
                contacts.Add(contact);
            }

            if (origin.SECU_CONT)
            {
                var contact = CopySecurityContact(origin);
                CopyContact(origin, ref contact);
                contacts.Add(contact);
            }

            //we dont generate maitenance record for now
            /*if (origin.MAINT_CONT)
            {
                var contact = CopyMaintContact(origin);
                CopyContact(origin, ref contact);
                contacts.Add(contact);
            }*/

            return contacts;
        }

        private static void CopyContact(COMPTEMP origin, ref Contact contact)
        {
            contact.NewManagerDate = origin.NEWMANDATE;
            contact.DaToCheck = origin.DATA_UPDAT;
            contact.DaToCheckInfo = origin.INFO_UPDAT;
            contact.ExtManagement = origin.NEED_INFO;
            contact.ReceptionName = origin.RECP_NAME;
        }

        private static Contact CopyCleaningContact(COMPTEMP origin)
        {
            return new Contact
            {
                CallFrequency = origin.CALL_ON_PM,
                Note = origin.NOTE_PAD,
                NextCall = origin.NEXT_CALL,
                LastCall = origin.LASTCONTAC,
                Code = Migration.GetCode(origin.SALES_REP),
                ContactPerson = CopyClContactPerson(origin),
                BusinessTypeId = (int)BusinessTypes.Cleaning
            };
        }

        private static Contact CopySecurityContact(COMPTEMP origin)
        {
            return new Contact
            {
                CallFrequency = origin.SE_CALLCYC,
                Note = origin.SE_CT_MEMO,
                NextCall = origin.SENEXTCALL,
                LastCall = origin.SELASTCALL,
                Code = Migration.GetCode(origin.SE_SAL_REP),
                ContactPerson = CopySeContactPerson(origin),
                BusinessTypeId = (int)BusinessTypes.Security
            };
        }

        private static Contact CopyMaintContact(COMPTEMP origin)
        {
            return new Contact
            {
                NextCall = null,
                LastCall = null,
                Code = null,
                BusinessTypeId = (int)BusinessTypes.Maintenance
            };
        }

        private static ContactPerson CopyClContactPerson(COMPTEMP origin)
        {
            if (string.IsNullOrEmpty(origin.TITLE) &&
                string.IsNullOrEmpty(origin.FIRST_NAME) &&
                string.IsNullOrEmpty(origin.LAST_NAME) &&
                string.IsNullOrEmpty(origin.EMAIL) &&
                string.IsNullOrEmpty(origin.POSITION) &&
                string.IsNullOrEmpty(origin.MOBILE) &&
                string.IsNullOrEmpty(origin.FAX_CLEAN) &&
                string.IsNullOrEmpty(origin.DIR_LINE) &&
                string.IsNullOrEmpty(origin.POBOX) &&
                string.IsNullOrEmpty(origin.POB_SUBURB) &&
                string.IsNullOrEmpty(origin.POB_STATE) &&
                string.IsNullOrEmpty(origin.POB_PCODE))
                return null;

            return new ContactPerson
            {
                Title = origin.TITLE,
                Firstname = origin.FIRST_NAME,
                Lastname = origin.LAST_NAME,
                Email = origin.EMAIL,
                Position = origin.POSITION,
                Mobile = origin.MOBILE,
                Fax = origin.FAX_CLEAN,
                DirectLine = origin.DIR_LINE,
                PoStreet = origin.POBOX,
                PoSuburb = origin.POB_SUBURB,
                PoState = origin.POB_STATE,
                PoPostcode = origin.POB_PCODE,
                CreateDate = DateTime.Today
            };
        }

        private static ContactPerson CopySeContactPerson(COMPTEMP origin)
        {
            if (string.IsNullOrEmpty(origin.SE_TITLE) &&
                string.IsNullOrEmpty(origin.SE_F_NAME) &&
                string.IsNullOrEmpty(origin.SE_L_NAME) &&
                string.IsNullOrEmpty(origin.SE_EMAIL) &&
                string.IsNullOrEmpty(origin.SE_POST) &&
                string.IsNullOrEmpty(origin.SE_DIRLINE))
                return null;

            return new ContactPerson
            {
                Title = origin.SE_TITLE,
                Firstname = origin.SE_F_NAME,
                Lastname = origin.SE_L_NAME,
                Email = origin.SE_EMAIL,
                Position = origin.SE_POST,
                DirectLine = origin.SE_DIRLINE,
                CreateDate = DateTime.Today
                //did not find a field for secruity fax
                //Fax =
            };
        }

        private static CleaningContract CopyCleaningContract(COMPTEMP origin)
        {
            if (string.IsNullOrEmpty(origin.CUR_CLN) &&
                string.IsNullOrEmpty(origin.CONT_QUOTE) &&
                origin.PRICE_PA == 0 &&
                string.IsNullOrEmpty(origin.UNSU_OPTIO) &&
                origin.TENDER_NEX == null &&
                origin.DATEQUOTED == null &&
                origin.CLEAN_FREQ == null &&
                origin.QUALI_NO == null)
                return null;

            return new CleaningContract
            {
                Contractor = origin.CUR_CLN,
                ContactDuringQuote = origin.CONT_QUOTE,
                PricePa = Convert.ToDecimal(origin.PRICE_PA),
                UnsuccessReason = origin.UNSU_OPTIO,
                ReviewDate = origin.TENDER_NEX,
                DateQuoted = origin.DATEQUOTED,
                CleaningFreq = origin.CLEAN_FREQ,
                QualifyingQuantity = origin.QUALI_NO
            };
        }

        private static SecurityContract CopySecurityContract(COMPTEMP origin)
        {
            if (string.IsNullOrEmpty(origin.CUR_SC) &&
                origin.SE_TENDER == null)
                return null;

            return new SecurityContract
            {
                Contarctor = origin.CUR_SC,
                ReviewDate = origin.SE_TENDER,
                GuardingPersonnel = origin.SC_GUAD,
                MobilePatrol = origin.SC_MOB_PAT,
                Conceirge = origin.CONCIERGE,
                ElectronicInstallation = origin.SC_CCTV,
                BackToBaseMonitoring = origin.SC_B2BMON,
                SecurityMaintenance = origin.SC_MAINTEN
            };
        }

        private static SiteGroup CopySiteGroup(COMPTEMP origin)
        {
            if (string.IsNullOrEmpty(origin.GRP_NAME) || 
                origin.SALES_REP == "PMS" || 
                origin.SALES_REP == "BMS")
                return null;

            var group = new SiteGroup
            {
                GroupName = origin.GRP_NAME,
                AgentComp = origin.AGENT_COMP
            };

            group.Type = "Group";
            return group;
        }
    }
}
