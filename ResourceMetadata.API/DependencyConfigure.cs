using Autofac;
using DataAccess.EntityFramework;
using DateAccess.Services;
using DateAccess.Services.Mappers;

namespace ResourceMetadata.API
{
    /// <summary>
    /// 
    /// </summary>
    public static class DependencyConfigure
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerBuilder"></param>
        public static void Init(ContainerBuilder containerBuilder)
        {
            MapperConfiguration.Configure();
            Register(containerBuilder);
        }

        private static void Register(ContainerBuilder containerBuilder)
        {
            ApplicationDependency.Register(containerBuilder);
            RepositoryDependency.Register(containerBuilder);
            ServiceDependency.Register(containerBuilder);
        }
    }
}
