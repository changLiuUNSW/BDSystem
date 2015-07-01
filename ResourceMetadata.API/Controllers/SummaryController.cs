using System.Web.Http;
using DateAccess.Services.StatisticService;

namespace ResourceMetadata.API.Controllers
{

    [RoutePrefix("api/summary")]
    [Authorize]
    public class SummaryController : ApiController
    {
        private readonly ISummaryService _summaryService;

        public SummaryController(ISummaryService summaryService)
        {
            _summaryService = summaryService;
        }
        [Route("contactperson/count")]
        public IHttpActionResult GetContactPersonCount()
        {
            var countMap = _summaryService.GetContactPersonCount();
            return Ok(new
            {
                data=countMap
            });
        }
        [Route("site/count")]
        public IHttpActionResult GetSiteCount()
        {
            var countMap = _summaryService.GetSiteCount();
            return Ok(new
            {
                data = countMap
            });
        }

    }
}
