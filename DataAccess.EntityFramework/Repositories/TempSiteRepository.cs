using System.Data.Entity;
using System.Linq;
using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.BD.Site;

namespace DataAccess.EntityFramework.Repositories
{
    public interface ITempSiteRepository : IRepository<TempSite>
    {
        TempSite CheckTempSiteExists(
              string company,
              string unit,
              string number,
              string street,
              string suburb,
              string postcode,
              string state);

        int RemoveNotUsedTempSite();
    }

    internal class TempSiteRepository : Repository<TempSite>, ITempSiteRepository
    {
        public TempSiteRepository(DbContext dbContext) : base(dbContext){ }

        public TempSite CheckTempSiteExists(string company, string unit, string number, string street, string suburb, string postcode,
            string state)
        {
            return DbSet.FirstOrDefault(l => l.Name.ToLower() == company.ToLower() &&
                                          l.Address.Unit.ToLower() == unit.ToLower() &&
                                          l.Address.Number.ToLower() == number.ToLower() &&
                                          l.Address.Street.ToLower() == street.ToLower() &&
                                          l.Address.Suburb.ToLower() == suburb.ToLower() &&
                                          l.State.ToLower() == state.ToLower() &&
                                          l.Postcode.ToLower() == postcode.ToLower());
          
        }

        public int RemoveNotUsedTempSite()
        {
            var unusedList = DbSet.Where(l => l.Quotes.Count == 0 && l.Costs.Count == 0).ToList();
            foreach (var unused in unusedList)
            {
                DbSet.Remove(unused);
            }
           return Save();
        }
    }
}
