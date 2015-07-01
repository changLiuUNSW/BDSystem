using System;
using System.Threading.Tasks;
using System.Web.Http;
using DataAccess.EntityFramework.Models.BD.Lead;
using DateAccess.Services.ContactService;
using Microsoft.AspNet.Identity;

namespace ResourceMetadata.API.Controllers
{
    /// <summary>
    ///     Data for workbook
    /// </summary>
    [RoutePrefix("api/lead")]
    [Authorize]
    public class LeadController : ApiController
    {
        private readonly ILeadService _leadService;

        /// <summary>
        ///     default constructor
        /// </summary>
        /// <param name="leadService"></param>
        public LeadController(ILeadService leadService)
        {
            _leadService = leadService;
        }

        /// <summary>
        ///     get all leads
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(new
            {
                data = _leadService.Get(true)
            });
        }

        /// <summary>
        ///     get specified lead with lead id
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var lead = _leadService.GetByKey(id);
            if (lead == null) return NotFound();
            return Ok(new
            {
                data = lead
            }
                );
        }

        [Route("qp/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateQp(int id, UpdateQpModel updateQpModel)
        {
            string user = User.Identity.GetUserName();
            Lead result = await _leadService.UpdateQp(id, user, updateQpModel);
            return Ok(new
            {
                data = result
            });
        }

        [Route("appointment/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAppointment(int id, AppointmentModel appointmentModel)
        {
            string user = User.Identity.GetUserName();
            Lead result = await _leadService.UpdateAppointment(id, user, appointmentModel);
            return Ok(new
            {
                data = result
            });
        }

        [Route("callback/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateCallback(int id, [FromBody] DateTime? callbackDate)
        {
            string user = User.Identity.GetUserName();
            Lead result = await _leadService.UpdateCallback(id, user, callbackDate);
            return Ok(new
            {
                data = result
            });
        }

        [Route("visited")]
        [HttpPost]
        public async Task<IHttpActionResult> Visited([FromBody] int id)
        {
            string user = User.Identity.GetUserName();
            Lead result = await _leadService.Visited(id, user);
            return Ok(new
            {
                data = result
            });
        }

        [Route("contacted")]
        [HttpPost]
        public async Task<IHttpActionResult> ContactNotSuccess([FromBody] int id)
        {
            string user = User.Identity.GetUserName();
            Lead result = await _leadService.ContactNotSuccess(id, user);
            return Ok(new
            {
                data = result
            });
        }

        [Route("cancel")]
        [HttpPost]
        public async Task<IHttpActionResult> CancelLead([FromBody] int id)
        {
            string user = User.Identity.GetUserName();
            Lead result = await _leadService.Cancel(id, user);
            return Ok(new
            {
                data = result
            });
        }


        [Route("qp")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllQpByZone(string zone)
        {
            var result = await _leadService.GetAllQpByZone(zone);
            return Ok(new
            {
                data = result
            });
        }

        /// <summary>
        ///     create a new lead
        /// </summary>
        /// <param name="lead"></param>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult PostLead(Lead lead)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            DateTime time = DateTime.Now;
            lead.CreatedDate = time;
            lead.LastUpdateDate = time;

            return Ok(new
            {
                data = _leadService.Add(lead)
            });
        }

        /// <summary>
        ///     delete the lead
        /// </summary>
        /// <returns></returns>
        [Route("{leadNo}")]
        public IHttpActionResult DeleteLead(int leadNo)
        {
            _leadService.Delete(leadNo);
            return Ok();
        }

        [Route("status")]
        public async Task<IHttpActionResult> GetAllStatus()
        {
            var statusList = await _leadService.GetAllLeadStatus();
            return Ok(new
            {
                data = statusList
            });
        }

        [Route("quote/{id}")]
        [HttpPost]
        public IHttpActionResult GenerateQuoteByLead(int id)
        {
            string user = User.Identity.GetUserName();
            var result = _leadService.GenerateQuote(id, user);
            return Ok(new
            {
                data=result
            });

        }
    }
}