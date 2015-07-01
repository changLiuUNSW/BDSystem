using System.Collections.Generic;
using DataAccess.EntityFramework.Models.BD.Allocation;

namespace DateAccess.Services.ViewModels
{
    public class ZoneAllocation
    {
        public string Zone { get; set; }
        public IEnumerable<Allocation> Allocations { get; set; } 
    }
}
