using System;
using System.Collections.ObjectModel;
using DataAccess.Console.Models;
using DataAccess.EntityFramework.Models.BD.Site;

namespace DataAccess.Console.Migration.DB.Mappers
{
    internal class SiteGroupMapper : IMigrationMapper
    {
        public object Map(COMPTEMP row)
        {
            if (string.IsNullOrEmpty(row.GRP_NAME) ||
                string.Compare(row.SALES_REP, "PMS", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(row.SALES_REP, "BMS", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return null;
            }

            return new SiteGroup
            {
                GroupName = row.GRP_NAME,
                AgentComp = row.AGENT_COMP,
                Type = "Group",
                ExternalManagers = new Collection<ExternalManager>(),
                Sites = new Collection<Site>()
            };
        }
    }
}
