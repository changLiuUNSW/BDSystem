using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Infrastructure
{
    public interface IDbContext
    {
        IDbSet<T> DbSet<T>() where T : class;
        int Save();
        Task<int> SaveAsync();
        DbEntityEntry DbEntry<T>(T entity) where T : class;
        void DbDispose();
        void EnableProxyCreation(bool set);
        void EnableLazyLoading(bool set);
        DbContextTransaction BeginTransaction(IsolationLevel isolation);
    }
}
