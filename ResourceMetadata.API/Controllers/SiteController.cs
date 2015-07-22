using System.Linq;
using System.Web.Http;
using AutoMapper;
using DateAccess.Services.SiteService;
using DateAccess.Services.ViewModels;
using Newtonsoft.Json.Linq;

namespace ResourceMetadata.API.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/site")]
    [Authorize]
    public class SiteController : ApiController
    {
        private readonly ISiteService _siteService;

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="siteService"></param>
        public SiteController(ISiteService siteService)
        {
            _siteService = siteService;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult Get(int id)
        {
            var site = _siteService.GetSite(id);
            if (site == null) return NotFound();
            var siteDto = Mapper.Map<SiteDTO>(site);

            return Ok(new
                {
                    data = siteDto
                }
                );
        }

        [Route("group")]
        [HttpPost]
        public IHttpActionResult UpdateGroup(SiteGroupModel siteGroupModel)
        {
            var site = _siteService.GetSite(siteGroupModel.SiteId);
            if (site.Groups.Any(l => l.Id == siteGroupModel.NewGroupId)) 
                return BadRequest("The group has already included this site");
            _siteService.UpdateGroup(siteGroupModel);
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Route("key")]
        public IHttpActionResult Get(string key)
        {
            var site = _siteService.GetSiteByKey(key);
            if (site == null) return BadRequest("key is not valid");
            var siteDto = Mapper.Map<SiteDTO>(site);
            return Ok(
                new
                {
                    data = siteDto
                }
                );
        }

        [Route("Admin")]
        [HttpPost]
        public IHttpActionResult UpdateFromAdmin(JObject json)
        {
            _siteService.UpdateSiteFromJson(json);
            return Ok();
        }
    }
}