using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Web.Http;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService;

namespace ResourceMetadata.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/telesale")]
    public class TelesaleController : ApiController
    {
        private readonly ITelesaleService _telesaleService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telesaleService"></param>
        public TelesaleController( ITelesaleService telesaleService  )
        {
            _telesaleService = telesaleService;
        }

        /// <summary>
        /// get all telesales
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult Get()
        {

            var data = _telesaleService.Repository.Include(x => x.Assignments);

            return Ok(new
            {
                data = data
            });
        }
        
        /// <summary>
        /// assign a call asignment to telesale
        /// </summary>
        /// <param name="assignment"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}/assign")]
        [HttpPost]
        public IHttpActionResult Assign(int id, Assignment assignment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var telesale = _telesaleService.GetByKey(id);
            if (telesale == null)
                return NotFound();

            try
            {
                _telesaleService.AddAssignment(id, assignment);
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    var exp = ex.InnerException as SqlException;

                    if (exp == null)
                    {
                        ex = ex.InnerException;
                        continue;
                    }

                    if (exp.Number == (int) SqlExceptionType.PKViolate) 
                        return
                            BadRequest("The area has already been assigned, refresh the page to see the latest update.");

                }   
            }

            return Ok(new
            {
                data = telesale
            });
        }

        /// <summary>
        /// remove call assignment from telelsale
        /// </summary>
        /// <param name="tsId"></param>
        /// <param name="id">assignment id</param>
        /// <param name="code"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        [Route("{tsId}/deassign")]
        [HttpDelete]
        public IHttpActionResult DeAssign(int tsId, int id, string code, string area)
        {
            var telesale = _telesaleService.GetByKey(tsId);
            if (telesale == null)
                return NotFound();

            try
            {
                _telesaleService.RemoveAssignment(id, code, area);
            }
            catch (DbUpdateConcurrencyException)
            {
                _telesaleService.Repository.Reload(telesale);
            }

            return Ok(new
            {
                data = telesale
            });
        }
    }
}
