using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using DateAccess.Services;
using DateAccess.Services.Infrastructure;

namespace ResourceMetadata.API
{
    public static class Bootstrapper
    {
        public static void Configure()
        {
            ConfigureAutofacContainer();
        }

        public static void ConfigureAutofacContainer()
        {
            var webApiContainerBuilder = new ContainerBuilder();
            ConfigureWebApiContainer(webApiContainerBuilder);
        }

        public static void ConfigureWebApiContainer(ContainerBuilder containerBuilder)
        {

            DependencyConfigure.Init(containerBuilder);
            containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            IContainer container = containerBuilder.Build();
            DomainEvents.SetEventBrokerStrategy(new AutofacEventBroker(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}