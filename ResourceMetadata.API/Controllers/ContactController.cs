using System;
using System.Linq;
using System.Web.Http;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService;
using DateAccess.Services.ContactService.Call;
using DateAccess.Services.ContactService.Call.Exceptions;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.ContactService.Call.Scripts.Info;
using DateAccess.Services.ContactService.Call.Scripts.Providers;
using DateAccess.Services.ContactService.Reports.Config;
using Newtonsoft.Json;
using ResourceMetadata.API.Json;
using ResourceMetadata.API.ViewModels;

namespace ResourceMetadata.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("api/contact")]
    public class ContactController : ApiController
    {
        private readonly IContactService _contactService;
        private readonly ICallService _callService;
        private readonly ICustomJsonConverter _customJsonConverter;

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="callService"></param>
        /// <param name="contactService"></param>
        public ContactController(IContactService contactService, ICallService callService)
        {
            _contactService = contactService;
            _callService = callService;
            _customJsonConverter = new CustomJsonConverter();
        }

        /// <summary>
        /// get a single contact by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            if (id <= 0)
                return BadRequest("Incorrect contact id");

            return Ok(new
            {
                data = _contactService.GetByKey(id)
            });
        }

        /// <summary>
        /// return next avaiable contact base on the underlying provider configuration
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("call/next")]
        [HttpPost]
        public IHttpActionResult NextCall(NextCallViewModel model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", new ArgumentNullException("model"));
                return BadRequest(ModelState);
            }

            try
            {
                CallDetail callDetail;
                _customJsonConverter.JsonSerializerSettings = Configuration.Formatters.JsonFormatter.SerializerSettings;

                if (!User.Identity.IsAuthenticated)
                    return Unauthorized();

                if (User.IsInRole("BD"))
                    callDetail = _callService.GetNextCall(CallType.BD, User.Identity.Name, model.SiteId,
                        model.LastCallId);
                else
                    callDetail = _callService.GetNextCall(CallType.Telesale, model.Initial, model.SiteId,
                        model.LastCallId);

                return Ok(new
                {
                    data = _customJsonConverter.ResolveObject(callDetail, new TelesaleContactJsonResolver())

                });
            }
            catch (UnfinishedCallException ex)
            {
                return Ok(new
                {
                    unfinished = true,
                    data = _customJsonConverter.ResolveObject(ex.CallDetail, new TelesaleContactJsonResolver())
                });
            }
        }

        /// <summary>
        /// get contact summary
        /// </summary>
        /// <returns></returns>
        [Route("Report")]
        public IHttpActionResult GetReport(ReportType type)
        {
            return Ok(new
            {
                data = _contactService.ReportProvider.Type(type).GetReport()
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("Report/History")]
        public IHttpActionResult GetReportHistory(ReportType type)
        {
            return Ok(new
            {
                data = _contactService.ReportProvider.Type(type).GetHistory()
            });
        }

        /// <summary>
        /// get contact call priority
        /// </summary>
        /// <returns></returns>
        [Route("call/priority")]
        public IHttpActionResult GetQpPriority()
        {
            var priority = _contactService.Priority;
            var result = priority.GetQueue().Select(x => new
            {
                x.Key.Initial,
                Priority = x.Value
            });

            return Ok(result);
        }

        /// <summary>
        /// update contact nodes
        /// </summary>
        /// <returns></returns>
        [Route("{contactId}/note")]
        public IHttpActionResult PutNote([FromUri]int contactId, [FromBody]string note)
        {
            var contact = _contactService.GetByKey(contactId);

            if (contact == null)
                return NotFound();

            return Ok(new
            {
                data = _contactService.UpdateNote(contactId, note)
            });
        }

        /// <summary>
        /// delete a call line from the contact
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{contactId}/CallLine/{id}")]
        public IHttpActionResult DeleteCallLine(int contactId, int id)
        {
            var rowAffected = _contactService.DeleteCallLine(contactId, id);

            if (rowAffected <= 0)
                return BadRequest();

            return Ok();
        }

        /// <summary>
        /// add a new contact line to the contact
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="callLine"></param>
        /// <returns></returns>
        [Route("{contactId}/CallLine")]
        public IHttpActionResult PostCallLine(int contactId, CallLine callLine)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _contactService.AddCallLine(contactId, callLine);

            if (result == null)
                return BadRequest();

            return Ok(new
            {
                data = result
            });
        }

        /// <summary>
        /// assign contactperson to the contact
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="contactPersonId"></param>
        /// <param name="targetContactId"></param>
        /// <param name="type">business if new contact needs to be created</param>
        /// <returns></returns>
        [Route("{contactId}/AssignContactPerson/{contactPersonId}")]
        public IHttpActionResult PutAssignContactPerson(int contactId, int contactPersonId, int targetContactId, BusinessTypes type)
        {
            var contact = _contactService.AssignContactperson(contactId, contactPersonId, targetContactId, type);

            return Ok(new
            {
                contact
            });
        }

        /// <summary>
        /// end a call by updating next call date and relavent information
        /// </summary>
        /// <returns></returns>
        [Route("call/end")]
        [HttpPost]
        public IHttpActionResult EndCall(EndCallViewModel model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", new ArgumentNullException("model"));
                return BadRequest(ModelState);
            }

            var actions =
                model.Actions["actions"].Select(
                    x => JsonConvert.DeserializeObject<ScriptAction>(x.ToString(), new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    })).ToList();

            var callProvider = User.IsInRole("BD")
                ? _callService.GetProvider(CallType.BD)
                : _callService.GetProvider(CallType.Telesale);

            callProvider.EndCall(
                model.SiteId,
                model.ContactId, 
                model.LeadPersonId,
                model.OccupiedId,
                model.Initial,
                model.RedirectUrl, 
                actions);

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("call/script/")]
        public IHttpActionResult GetScripts(ScriptType type)
        {
            var script = ScriptProvider.GetScript(type);
            return Ok(new
            {
                data = script
            });
        }
    }
}