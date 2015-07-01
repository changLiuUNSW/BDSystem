using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common.Paging
{
    public class PagingHelper
    {
        public static int NumSkips(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
                return 0;

            return (page - 1) * pageSize;
        }

        public static int NumPages(int pageSize, int total)
        {
            return pageSize > 0 ? (int)Math.Ceiling((double)total / pageSize) : 0;
        }
    }
}
