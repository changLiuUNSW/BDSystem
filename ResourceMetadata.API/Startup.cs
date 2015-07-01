using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using ResourceMetadata.API;

[assembly: OwinStartup(typeof (Startup))]

namespace ResourceMetadata.API
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
           ConfigureAuth(app);
           app.UseCors(CorsOptions.AllowAll);
            
        }
    }
}