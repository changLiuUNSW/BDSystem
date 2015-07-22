using System;
using System.Collections.ObjectModel;
using DataAccess.Console.Models;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Site;

namespace DataAccess.Console.Migration.DB.Mappers
{
    internal class SiteMapper : ConfigurableMapper, IMigrationMapper
    {
        public SiteMapper(MigrationConfiguration config) : base(config) { } 

        public object Map(COMPTEMP row)
        {
            var site = new Site
            {
                Key = row.KEY,
                Unit = row.UNIT,
                Number = row.NUMBER,
                Street = row.STREET,
                Suburb = row.SUBURB,
                State = row.STATE,
                Postcode = row.P_CODE,
                Name = row.COMPANY,
                BuildingName = row.BUILD_ID,
                //PropertyManagerName = row.AGENT_COMP,
                PropertyManaged = row.MANBYAGENT,
                PMSite = row.PM_SITE,
                Phone = row.PHONE,
                InHouse = row.IN_HOUSE,
                Size = Configuration.MigrationSizeConverter.Convert(row.SALES_REP),
                TsToCall = row.TSTOCALL,
                Qualification = row.QUALI_NO,
                CleaningContract = MapCleaningContract(row),
                SecurityContract = MapSecurityContract(row),
                Contacts = new Collection<Contact>(),
                ContactPersons = new Collection<ContactPerson>(),
                Groups = new Collection<SiteGroup>()
            };

            return site;
        }

        private CleaningContract MapCleaningContract(COMPTEMP row)
        {
            if (string.IsNullOrEmpty(row.CUR_CLN) &&
                string.IsNullOrEmpty(row.CONT_QUOTE) &&
                row.PRICE_PA == 0 &&
                string.IsNullOrEmpty(row.UNSU_OPTIO) &&
                row.TENDER_NEX == null &&
                row.DATEQUOTED == null &&
                row.CLEAN_FREQ == null &&
                row.QUALI_NO == null)
                return null;

            return new CleaningContract
            {
                Contractor = row.CUR_CLN,
                ContactDuringQuote = row.CONT_QUOTE,
                PricePa = Convert.ToDecimal(row.PRICE_PA),
                UnsuccessReason = row.UNSU_OPTIO,
                ReviewDate = row.TENDER_NEX,
                DateQuoted = row.DATEQUOTED,
                CleaningFreq = row.CLEAN_FREQ,
                QualifyingQuantity = row.QUALI_NO
            };
        }

        private SecurityContract MapSecurityContract(COMPTEMP row)
        {
            if (string.IsNullOrEmpty(row.CUR_SC) &&
                row.SE_TENDER == null)
                return null;

            return new SecurityContract
            {
                Contarctor = row.CUR_SC,
                ReviewDate = row.SE_TENDER,
                GuardingPersonnel = row.SC_GUAD,
                MobilePatrol = row.SC_MOB_PAT,
                Conceirge = row.CONCIERGE,
                ElectronicInstallation = row.SC_CCTV,
                BackToBaseMonitoring = row.SC_B2BMON,
                SecurityMaintenance = row.SC_MAINTEN
            };
        }
    }
}
