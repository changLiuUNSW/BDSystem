using System.Collections.Generic;
using DataAccess.EntityFramework.Models.BD.Allocation;

namespace DateAccess.Services.ContactService.Reports.Comparer
{
    public class AllocationComparer : IEqualityComparer<Allocation>
    {
        public bool Equals(Allocation x, Allocation y)
        {
            if (x.Size == y.Size && x.Zone == y.Zone)
                return true;

            return false;
        }

        public int GetHashCode(Allocation obj)
        {
            var code = obj.Size + obj.Zone;
            return code.GetHashCode();
        }
    }
}
