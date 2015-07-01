using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common.Paging
{
    public class Paging<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public string NextLink { get; set; }
        public string PrevLink { get; set; }
        public IList<T> Data { get; set; }
    }

    
}
