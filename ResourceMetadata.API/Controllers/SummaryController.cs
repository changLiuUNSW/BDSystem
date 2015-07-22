using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DataAccess.EntityFramework.Repositories;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.StatisticService;

namespace ResourceMetadata.API.Controllers
{

    /// <summary>
    /// provide table row count overview
    /// </summary>
    [RoutePrefix("api/summary")]
    //[Authorize]
    public class SummaryController : ApiController
    {
        private readonly ISummaryService _summaryService;

        /// <summary>
        /// default contructor
        /// </summary>
        /// <param name="summaryService"></param>
        public SummaryController(ISummaryService summaryService)
        {
            _summaryService = summaryService;
        }

        /// <summary>
        /// return contact person count for each business type category
        /// </summary>
        /// <returns></returns>
        [Route("contactperson/count")]
        public IHttpActionResult GetContactPersonCount()
        {
            List<SummaryCount> contactPersonCount;

            if (User.Identity.IsAuthenticated && User.IsInRole("BD"))
                contactPersonCount = _summaryService.ContactPersonCountForBD(User.Identity.Name);
            else
                contactPersonCount = _summaryService.ContactPersonCount();

            return Ok(new
            {
                data = contactPersonCount
            });
        }

        /// <summary>
        /// return site count for eac hbusiness type category 
        /// </summary>
        /// <returns></returns>
        [Route("site/count")]
        public IHttpActionResult GetSiteCount()
        {
            List<SummaryCount> siteCount;

            if (User.Identity.IsAuthenticated && User.IsInRole("BD"))
                siteCount = _summaryService.SiteCountForBD(User.Identity.Name);
            else
                siteCount = _summaryService.SiteCount();

            return Ok(new
            {
                data = siteCount
            });
        }
    }
}
