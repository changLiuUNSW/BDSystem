using System.Web.Http;
using System.Web.Mvc;

namespace ResourceMetadata.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Get home
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var apiExplorer = GlobalConfiguration.Configuration.Services.GetApiExplorer();
            return View(apiExplorer.ApiDescriptions);
        }
    }
}