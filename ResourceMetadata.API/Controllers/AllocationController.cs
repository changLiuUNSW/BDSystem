using System.Linq;
using System.Web.Http;
using DataAccess.EntityFramework.Models.BD.Allocation;
using DateAccess.Services.ContactService;

namespace ResourceMetadata.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/allocation")]
    public class AllocationController : ApiController
    {
        private readonly IAllocationService _allocationService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allocationService"></param>
        public AllocationController(IAllocationService allocationService)
        {
            _allocationService = allocationService;
        }

        /// <summary>
        /// post api/allocation
        /// </summary>
        /// <param name="allocation"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostAllocation(Allocation allocation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _allocationService.Add(allocation);
            return Ok(new
            {
                data = allocation
            });
        }

        /// <summary>
        /// put api/allocation
        /// </summary>
        /// <param name="allocation"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public IHttpActionResult PutAllocation(Allocation allocation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _allocationService.Update(allocation);
            return Ok();
        }

        /// <summary>
        /// delete api/allocation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult DeleteAllocation(int id)
        {
            _allocationService.Delete(id);
            return Ok();
        }

        /// <summary>
        /// get api/allocation
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult GetAllocation()
        {
            return Ok(new
            {
                data = _allocationService.Get(true)
            });
        }

        /// <summary>
        /// return list of zone/size that have not been assigned to anyone
        /// </summary>
        /// <returns></returns>
        [Route("idleZones")]
        public IHttpActionResult GetIdleZones()
        {
            return Ok(new
            {
                data = _allocationService.IdleZones().GroupBy(x => x.Zone).Select(x => new
                {
                    zone = x.Key,
                    sizes = x.Select(y=>y.Size)
                })
            });
        }
    }
}
