using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using DataAccess.Console.Models;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Site;

namespace DataAccess.Console.Migration.DB.Mappers
{
    internal class TenentMapper : ConfigurableMapper, IMigrationMapper
    {
        public TenentMapper(MigrationConfiguration config) : base(config) { }

        public object Map(COMPTEMP row)
        {
            if (string.Compare(row.SALES_REP, "PMS", StringComparison.OrdinalIgnoreCase) != 0)
                return null;

            //tenants are saved as sites
            var tenants = new List<Site>();

            if (!string.IsNullOrEmpty(row.M_TENANT1))
            {
                var tenant = MapTenant(row);
                MatchTenantInfo(tenant, row.M_TENANT1, row.STATE);
                tenants.Add(tenant);
            }

            if (!string.IsNullOrEmpty(row.M_TENANT2))
            {
                var tenant = MapTenant(row);
                MatchTenantInfo(tenant, row.M_TENANT2, row.STATE);
                tenants.Add(tenant);
            }

            return tenants;
        }

        private string PhoneCodeRegex(string state)
        {
            if (Configuration.PhoneCodes.ContainsKey(state))
            {
                return string.Format(@"\(?{0}\)?\s*\d{{4,8}}?\s*\d{{4,8}}?", Configuration.PhoneCodes[state]);
            }

            return null;
        }

        private string MobileCodeRegiex()
        {
            if (Configuration.PhoneCodes.ContainsKey("Mobile"))
            {
                return string.Format(@"{0}\d{{2,2}}\s?\d{{3,3}}\s?\d{{3,3}}", Configuration.PhoneCodes["Mobile"]);
            }

            return null;
        }

        private Site MapTenant(COMPTEMP row)
        {
            return new Site
            {
                Key = Configuration.TenantKey,
                Number = row.NUMBER,
                Street = row.STREET,
                Suburb = row.SUBURB,
                State = row.STATE,
                Postcode = row.P_CODE,
                BuildingName = row.BUILD_ID,
                PropertyManaged = true,
                Contacts = new Collection<Contact>(),
                ContactPersons = new Collection<ContactPerson>(),
                Groups = new Collection<SiteGroup>()
            };
        }

        private void MatchTenantInfo(Site tenant, string tenantInfo, string state)
        {
            string phone = "", name = "";
            var success = MatchTenantInfo(tenantInfo, PhoneCodeRegex(state.ToUpper()), ref phone, ref name);

            if (!success)
                success = MatchTenantInfo(tenantInfo, MobileCodeRegiex(), ref phone, ref name);

            if (success)
            {
                tenant.Phone = phone;
                tenant.Name = name;
            }
        }

        private bool MatchTenantInfo(string tenantString, string regexExpression, ref string phone, ref string name)
        {
            if (string.IsNullOrEmpty(tenantString) || string.IsNullOrEmpty(regexExpression))
                return false;

            var regexMatch = Regex.Match(tenantString, regexExpression, RegexOptions.IgnoreCase);

            if (regexMatch.Success)
            {
                var matched = regexMatch.Groups[regexMatch.Groups.Count - 1].Value;

                phone = matched.Length > 15 ? matched.Substring(15) : matched;
                name = Regex.Replace(tenantString, regexExpression, "");
            }

            return regexMatch.Success;
        }
    }
}
