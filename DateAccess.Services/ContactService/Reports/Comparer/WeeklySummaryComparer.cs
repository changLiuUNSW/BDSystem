using System.Collections.Generic;
using DataAccess.Common.Contact;

namespace DateAccess.Services.ContactService.Reports.Comparer
{
    public class WeeklySummaryComparer :IEqualityComparer<WeeklySummary>
    {
        public bool Equals(WeeklySummary x, WeeklySummary y)
        {
            if (x.Size == y.Size && x.Zone == y.Zone)
                return true;

            return false;
        }

        public int GetHashCode(WeeklySummary obj)
        {
            var code = obj.Size + obj.Zone;
            return code.GetHashCode();
        }
    }
}
