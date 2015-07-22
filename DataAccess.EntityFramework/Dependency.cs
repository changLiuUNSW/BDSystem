using System.Data.Entity;
using Autofac;
using DataAccess.EntityFramework.DbContexts;
using DataAccess.EntityFramework.Infrastructure;

namespace DataAccess.EntityFramework
{
    public static class RepositoryDependency
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<SiteResourceEntities>().As<DbContext>().InstancePerRequest();
            containerBuilder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerRequest();
            containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
        }
    }
}
