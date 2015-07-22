using System.Data.Entity;
using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.Quote.Cost;

namespace DataAccess.EntityFramework.Repositories
{
    public interface IQuoteCostRepository : IRepository<Cost>
    {
    }

    internal class QuoteCostRepository : Repository<Cost>, IQuoteCostRepository
    {
        public QuoteCostRepository(DbContext dbContext) : base(dbContext){}
    }

}