using System;
using System.Collections.Generic;
using System.Web.Http;
using DataAccess.EntityFramework.Models.BD.Allocation;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService;

namespace ResourceMetadata.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/salesbox")]
    public class SalesBoxController : ApiController
    {
        private readonly IAreaService _areaService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areaService"></param>
        public SalesBoxController(IAreaService areaService)
        {
            _areaService = areaService;
        }

        /// <summary>
        /// return all unique states
        /// </summary>
        /// <returns></returns>
        [Route("state")]
        public IHttpActionResult GetState()
        {
            return Ok(new
            {
                data = _areaService.States
            });
        }

        /// <summary>
        /// return all unique zones
        /// </summary>
        /// <returns></returns>
        [Route("zone")]
        public IHttpActionResult GetZone()
        {
            return Ok(new
            {
                data = _areaService.Zones
            });
        }

        /// <summary>
        /// return zone with their respective allocation if any
        /// </summary>
        /// <returns></returns>
        [Route("zoneAllocation")]
        public IHttpActionResult GetZoneAllocation()
        {
            return Ok(new
            {
                data = new
                {
                    sizes = new [] {"008", "025", "050", "120"},
                    zones = _areaService.ZoneAllocations
                }
            });
        }

        /// <summary>
        /// add a new salesbox
        /// </summary>
        /// <param name="salesBox"></param>
        /// <returns></returns>
        public IHttpActionResult Post(SalesBox salesBox)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new
            {
                data = _areaService.Add(salesBox)
            });
        }

        /// <summary>
        /// delete the salesbox
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult Delete(string postcode, string state)
        {
            _areaService.Delete(postcode, state);
            return Ok();
        }

        /// <summary>
        /// update a single salesboxes
        /// </summary>
        /// <param name="salesBox"></param>
        /// <returns></returns>
        public IHttpActionResult Put(SalesBox salesBox)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _areaService.Update(salesBox);
            return Ok();
        }

        /// <summary>
        /// return salesboxes base on the query parameter provided
        /// </summary>
        /// <param name="postCode"></param>
        /// <param name="state"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IHttpActionResult Get(string postCode = null, string state = null, int? take=null)
        {
            var salesboxes = _areaService.Get(postCode, state, take);

            return Ok(new
            {
                data = salesboxes
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="postcode"></param>
        /// <returns></returns>
        public IHttpActionResult Get(int page, int pageSize, string postcode = null)
        {
            return Ok(new
            {
                data = _areaService.GetPage(page, pageSize, postcode)
            });
        }
    }
}