using System.Collections.Generic;
using DataAccess.EntityFramework.Models.BD.Lead;
using DateAccess.Services.ContactService.Call.Models;

namespace DateAccess.Services.ContactService.Call.Stats
{
    public interface ILeadPersonStatsProvider
    {
        IList<LeadPersonStat> GetStats(IList<LeadPersonal> param);
    }
}
