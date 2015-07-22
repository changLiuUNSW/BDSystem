using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using DataAccess.EntityFramework.Models.BD.Lead;
using DateAccess.Services.ContactService;
using Newtonsoft.Json;
using ResourceMetadata.API.Json;

namespace ResourceMetadata.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/leadpersonal")]
    public class LeadPersonalController : ApiController
    {
        private readonly ILeadPersonalService _leadPersonalService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leadPersonalService"></param>
        public LeadPersonalController(ILeadPersonalService leadPersonalService)
        {
            _leadPersonalService = leadPersonalService;
        }

        /// <summary>
        /// get all qp lead priority
        /// </summary>
        /// <param name="stats">include contact statistics for each qp</param>
        /// <returns></returns>
        public IHttpActionResult Get(bool stats = false)
        {
            var leadPerson = _leadPersonalService.Repository.Include(x => x.LeadGroup, x => x.Leads);

            var converter = new CustomJsonConverter(new AllocationJsonResolver(),
                Configuration.Formatters.JsonFormatter.SerializerSettings);

            if (stats)
            {
                var personStats = _leadPersonalService.StatsProvider.GetStats(leadPerson);
                var groups = _leadPersonalService.GetLeadGroup();

                var resolved = converter.ResolveArray(leadPerson);

                return Ok(new
                {
                    data = new
                    {
                        persons = converter.ResolveArray(leadPerson),
                        stats = personStats,
                        groups = groups
                    }
                });
            }
                
            return Ok(new
            {
                data = converter.ResolveArray(leadPerson)
            });
        }

        /// <summary>
        /// return name and id of lead personals
        /// </summary>
        /// <returns></returns>
        [Route("Names")]
        public IHttpActionResult GetName()
        {
            return Ok(new
            {
                data = new
                {
                    persons = _leadPersonalService.GetNames(),
                    groups = _leadPersonalService.GetLeadGroup()
                }
            });
        }

        /// <summary>
        /// return a single lead priority person
        /// </summary>
        /// <param name="id"></param>
        /// <param name="getStats"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Get(int id, bool getStats = false)
        {
            var person = _leadPersonalService.GetByKey(id);

            if (!getStats)
            {
                return Ok(new
                {
                    data = person
                });
            }

            //var stats = _leadPersonalService.StatsProvider.GetStats(person);
            return Ok(new
            {
                data = new 
                {
                    person,
                    //stats
                }
            });
        }

        /// <summary>
        /// update the qp lead priority
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IHttpActionResult Put(LeadPersonal model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new
            {
                data = _leadPersonalService.Update(model)
            });
        }

        /// <summary>
        /// update a list of qp lead priority
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [Route("Batch")]
        public IHttpActionResult Put(IList<LeadPersonal> models)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new
            {
                data = _leadPersonalService.InsertOrUpdate(models)
            });
        }

        /// <summary>
        /// create a new qp lead priority
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IHttpActionResult Post(LeadPersonal model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new
            {
                data = _leadPersonalService.Add(model)
            });
        }

        /// <summary>
        /// delete the qp lead priority
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{Id}")]
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Incorrect id");

            return Ok(new
            {
                data = _leadPersonalService.Delete(id)
            });
        }

        /// <summary>
        /// get lead shift information
        /// </summary>
        /// <returns></returns>
        [Route("shift")]
        public IHttpActionResult GetShift()
        {
            return Ok(new
            {
                data = _leadPersonalService.GetLeadShift()
            });
        }

        /// <summary>
        /// update lead shift information
        /// </summary>
        /// <returns></returns>
        [Route("shift")]
        public IHttpActionResult PutShift(LeadShiftInfo shift)
        {
            _leadPersonalService.UpdateShiftInfo(shift);

            return Ok(new 
            {
                data = shift
            });
        }
    }
}
