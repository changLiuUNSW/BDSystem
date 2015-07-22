using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;
using ResourceMetadata.API.Filters;

namespace ResourceMetadata.API
{
    /// <summary>
    /// web route config
    /// </summary>
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
        

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
//            config.SuppressDefaultHostAuthentication();
//            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
//            config.Filters.Add(new AuthorizeAttribute());
            config.Filters.Add(new GlobalExceptionFilter());
            
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            
            //type name handling for covariant collection
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            //Fource parse to local time
            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            //In case of circular refenrence
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            
            //enable tracing
//            var tracer = config.EnableSystemDiagnosticsTracing();
//            tracer.IsVerbose = true;
//            tracer.MinimumLevel = TraceLevel.Debug;
        }
    }
}
