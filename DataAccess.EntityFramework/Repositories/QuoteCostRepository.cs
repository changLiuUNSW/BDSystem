using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.Quote.Cost;

namespace DataAccess.EntityFramework.Repositories
{
    public interface IQuoteCostRepository : IRepository<Cost>
    {
    }

    internal class QuoteCostRepository : Repository<Cost>, IQuoteCostRepository
    {
        public QuoteCostRepository(IDbContext dbContext) : base(dbContext){}
    }

}