using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Elmah.Contrib.WebApi;

namespace ResourceMetadata.API
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/fwlink/?LinkId=301868
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            Bootstrapper.Configure();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //Add Elmah. To visit: domain/elmah.axd
            GlobalConfiguration.Configuration.Filters.Add(new ElmahHandleErrorApiAttribute());
            //Always display error detail
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        }
    }
}