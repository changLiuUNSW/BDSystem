using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using AutoMapper;
using DataAccess.EntityFramework.Models.BD.Site;
using DateAccess.Services.SiteService;
using DateAccess.Services.ViewModels;

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
            var siteGroup=_siteGroupService.GetById(id);
            if (siteGroup == null) return NotFound();
            return Ok(new
            {
                data = siteGroup
            });
        }
        [Route("")]
        public IHttpActionResult Get()
        {
            var grouplist = _siteGroupService.GetWithInclude(new Expression<Func<SiteGroup, object>>[]
                {
                    l=>l.Sites
                }, true);
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
            var group = _siteGroupService.GetById(id);
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
            return Ok(new
            {
                data = groups
            });
        }

        [Route("{id}/site")]
        [HttpDelete]
        public IHttpActionResult RemoveSite(int id, [FromUri]int[] siteIds)
        {
            var group = _siteGroupService.GetById(id);
            if (group == null) return BadRequest("Group " + id + " does not exist");
            _siteGroupService.RemoveSites(id,siteIds);
            return Ok();
        }
    }
}
