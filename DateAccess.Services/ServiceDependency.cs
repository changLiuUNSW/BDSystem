﻿using Autofac;
using DateAccess.Services.ContactService;
using DateAccess.Services.ContactService.Call;
using DateAccess.Services.EventHandlers;
using DateAccess.Services.Infrastructure;
using DateAccess.Services.MailService;
using DateAccess.Services.QuoteService;
using DateAccess.Services.QuoteService.Helper;
using DateAccess.Services.SearchService;
using DateAccess.Services.SiteService;
using DateAccess.Services.StatisticService;

namespace DateAccess.Services
{
    public static class ServiceDependency
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(typeof (SendLeadAppointmentEmail).Assembly)
                .Where(l => l.IsClosedTypeOf(typeof (IHandle<>)))
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            containerBuilder.RegisterType<ContactService.ContactService>().As<IContactService>().InstancePerRequest();
            containerBuilder.RegisterType<AllocationService>().As<IAllocationService>().InstancePerRequest();
            containerBuilder.RegisterType<QuoteEmailService>().As<IQuoteEmailService>().InstancePerRequest();
            containerBuilder.RegisterType<EmailHelper>().As<IEmailHelper>().InstancePerRequest();
            containerBuilder.RegisterType<ContactPersonService>().As<IContactPersonService>().InstancePerRequest();
            containerBuilder.RegisterType<LeadPersonalService>().As<ILeadPersonalService>().InstancePerRequest();
            containerBuilder.RegisterType<AreaService>().As<IAreaService>().InstancePerRequest();
            containerBuilder.RegisterType<LeadService>().As<ILeadService>().InstancePerRequest();
            containerBuilder.RegisterType<SiteService.SiteService>().As<ISiteService>().InstancePerRequest();
            containerBuilder.RegisterType<SiteGroupService>().As<ISiteGroupService>().InstancePerRequest();
            containerBuilder.RegisterType<SiteAddressHelper>().As<ISiteAddressHelper>().InstancePerRequest();
            containerBuilder.RegisterType<QuoteService.QuoteService>().As<IQuoteService>().InstancePerRequest();
            containerBuilder.RegisterType<QuoteCostService>().As<IQuoteCostService>().InstancePerRequest();
            containerBuilder.RegisterType<SearchService.SearchService>().As<ISearchService>().InstancePerRequest();
            containerBuilder.RegisterType<SaleBoxService>().As<ISaleBoxService>().InstancePerRequest();
            containerBuilder.RegisterType<TelesaleService>().As<ITelesaleService>().InstancePerRequest();
            containerBuilder.RegisterType<CallService>().As<ICallService>().InstancePerRequest();
            containerBuilder.RegisterType<SummaryService>().As<ISummaryService>().InstancePerRequest();
        }
    }
}
