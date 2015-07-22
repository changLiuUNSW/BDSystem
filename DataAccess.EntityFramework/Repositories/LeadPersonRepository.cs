using System.Data.Entity;
using System.Linq;
using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.Quad;

namespace DataAccess.EntityFramework.Repositories
{
    public interface ILeadPersonRepository : IRepository<LeadPersonal>
    {
        LeadPersonal GetFromPhoneBook(string loginName);
    }

    internal class LeadPersonRepository : Repository<LeadPersonal>, ILeadPersonRepository
    {
        internal LeadPersonRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public LeadPersonal GetFromPhoneBook(string loginName)
        {
            var person = DataContext.Set<QuadPhoneBook>()
                .Where(x => x.LoginName == loginName)
                .Join(DbSet, x => x.Intial, x => x.Initial, (book, personal) => personal);

            return person.SingleOrDefault();
        }
    }
}
