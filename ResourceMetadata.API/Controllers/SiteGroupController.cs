using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using DataAccess.EntityFramework.Models.BD.Site;
using DateAccess.Services.SiteService;
using DateAccess.Services.ViewModels;
using ResourceMetadata.API.Json;

namespace ResourceMetadata.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/group")]
    [Authorize]
    public class SiteGroupController : ApiController
    {
        private readonly ISiteGroupService _siteGroupService;

          /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="siteGroupService"></param>
        public SiteGroupController(ISiteGroupService siteGroupService)
        {
            _siteGroupService = siteGroupService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var siteGroup=_siteGroupService.Repository.Get(id);
            if (siteGroup == null) return NotFound();

            var converter = new CustomJsonConverter(new SiteGroupJsonResolver(),
                Configuration.Formatters.JsonFormatter.SerializerSettings);
            var data = converter.ResolveObject(siteGroup);
            return Ok(new
            {
                data = data
            });
        }
        [Route("")]
        public IHttpActionResult Get()
        {
            var grouplist = _siteGroupService.Repository.Include(x => x.Sites);
            var grouplistDto = Mapper.Map<List<SiteGroupDTO>>(grouplist);
            return Ok(new
            {
                data = grouplistDto
            });
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteGroup"></param>
        /// <returns></returns>
		[Route("")]
        public IHttpActionResult Post(SiteGroup siteGroup)
        {
            if (!ModelState.IsValid)return BadRequest(ModelState);
            _siteGroupService.Add(siteGroup);
            return Ok(new
            {
                data=siteGroup
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteGroup"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPut]
        public IHttpActionResult Put(SiteGroup siteGroup)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _siteGroupService.Update(siteGroup);
            return Ok(new
            {
                data=siteGroup
            });
        }


        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteSiteGroup(int id)
        {
            var group = _siteGroupService.GetByKey(id);
            if (group == null) return NotFound();
            if (group.Sites.Any())
            {
                return BadRequest("This group contains multiple sites, you cannot delete it directly");
            }
            _siteGroupService.Delete(id);
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpGet]
        public IHttpActionResult SearchSiteGroup(string type=null, string code=null)
        {
            var groups = _siteGroupService.Search(type, code);
            var converter = new CustomJsonConverter(new SiteGroupJsonResolver(),
                Configuration.Formatters.JsonFormatter.SerializerSettings);
            return Ok(new
            {
                data = converter.ResolveArray(groups)
            });
        }

        [Route("{id}/site")]
        [HttpDelete]
        public IHttpActionResult RemoveSite(int id, [FromUri]int[] siteIds)
        {
            var group = _siteGroupService.GetByKey(id);
            if (group == null) return BadRequest("Group " + id + " does not exist");
            _siteGroupService.RemoveSites(id,siteIds);
            return Ok();
        }
    }
}
