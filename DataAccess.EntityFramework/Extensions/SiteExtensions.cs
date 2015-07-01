using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.EntityFramework.Expressions;
using DataAccess.EntityFramework.Models.BD.Allocation;
using DataAccess.EntityFramework.Models.BD.Site;

namespace DataAccess.EntityFramework.Extensions
{
    internal static class SiteExtensions
    {
        /// <summary>
        /// filter site by allocation with zone and size
        /// </summary>
        /// <param name="source"></param>
        /// <param name="allocations"></param>
        /// <returns></returns>
        public static IQueryable<Site> FilterByAllocation(this IQueryable<Site> source, IEnumerable<Allocation> allocations)
        {
            Expression<Func<Site, bool>> expression = null;
            foreach (var allocation in allocations)
            {
                var size = allocation.Size;
                var zone = allocation.Zone;
                Expression<Func<Site, bool>> expr = x => x.Size == size && x.SalesBox.Zone == zone;

                if (expression == null)
                    expression = expr;
                else
                    expression = expression.Or(expr);
            }

            if (expression != null)
                return source.Where(expression);

            return source;
        }
    }
}
