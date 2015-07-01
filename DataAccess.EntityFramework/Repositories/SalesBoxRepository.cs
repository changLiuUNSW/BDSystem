using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DataAccess.Common.Paging;
using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.BD.Allocation;

namespace DataAccess.EntityFramework.Repositories
{
    public interface ISalesBoxRepository : IRepository<SalesBox>
    {
        IList<SalesBox> GetSaleBox(string postCode = null, string state = null, int? take = null);
        Paging<SalesBox> Page(int page, int pageSize);
        Paging<SalesBox> Page(int page, int pageSize, string postcode);
    }
    internal class SalesBoxRepository : Repository<SalesBox>, ISalesBoxRepository
    {
        internal SalesBoxRepository(IDbContext dbContext) : base(dbContext)
        {
        }

        public Paging<SalesBox> Page(int page, int pageSize)
        {
            var count = DbSet.Count();
            var totalPages = PagingHelper.NumPages(pageSize, count);

            var result = new Paging<SalesBox>
            {
                TotalCount = count,
                TotalPages = totalPages
            };

            if (page <= 0)
                return result;

            result.Data =
                DbSet.OrderBy(x => x.Postcode).Skip(PagingHelper.NumSkips(page, pageSize)).Take(pageSize).ToList();

            return result;
        }

        public Paging<SalesBox> Page(int page, int pageSize, string postcode)
        {
            var count = DbSet.Count(x=>x.Postcode.Contains(postcode));
            var totalPages = PagingHelper.NumPages(pageSize, count);

            var result = new Paging<SalesBox>
            {
                TotalCount = count,
                TotalPages = totalPages
            };

            if (page <= 0)
                return result;

            result.Data =
                DbSet.Where(x => x.Postcode.Contains(postcode))
                    .OrderBy(x => x.Postcode)
                    .Skip(PagingHelper.NumSkips(page, pageSize))
                    .Take(pageSize)
                    .ToList();

            return result;
        }

        public IList<SalesBox> GetSaleBox(string postCode = null, string state = null, int? take = null)
        {
            var query = DbSet.AsQueryable();
            if (!string.IsNullOrEmpty(postCode)) query = query.Where(l => l.Postcode.StartsWith(postCode));
            if (!string.IsNullOrEmpty(state)) query = query.Where(l => l.State.StartsWith(state));

            return take == null ? query.ToList() : query.Take(Convert.ToInt32(take)).ToList();
        }
    }
}