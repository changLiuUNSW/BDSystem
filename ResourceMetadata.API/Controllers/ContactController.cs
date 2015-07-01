using System.Linq;
using System.Web.Http;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService;
using DateAccess.Services.ContactService.Call;
using DateAccess.Services.ContactService.Call.Exceptions;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.ContactService.Reports.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceMetadata.API.Json;

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
            _customJsonConverter = new TelesaleContactJsonConverter();
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
        /// <param name="type">type of provider to use</param>
        /// <param name="initial">if initial is supplied and siteId is not, provider will use initial to find next call</param>
        /// <param name="siteId">if siteId is supplied, provider will use siteId to find next call and ignoring the value in initial</param>
        /// <param name="lastCallId">if lastCallId is supplied, it will be removed from the occupied call list prior to finding the next call</param>
        /// <returns></returns>
        [Route("call/next")]
        public IHttpActionResult GetNext(CallTypes type, string initial = null, int? siteId = null, int? lastCallId = null)
        {

            try
            {
                CallDetail data;
                switch (type)
                {
                    case CallTypes.BD:
                        if (!User.Identity.IsAuthenticated)
                            return BadRequest("Undefined user");
                        var name = User.Identity.Name;
                        data = _callService.GetNextCall(type, name, siteId, lastCallId);
                        break;
                    case CallTypes.Telesale:
                        data = _callService.GetNextCall(type, initial, siteId, lastCallId);
                        break;
                    default:
                        data = null;
                        break;
                }

                return Ok(new
                {
                    data = _customJsonConverter.Resolve(data)

                });

            }
            catch (UnfinishedCallException ex)
            {
                return Ok(new
                {
                    unfinished = true,
                    data = _customJsonConverter.Resolve(ex.CallDetail)
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
        ///  update single field of the contact
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
        [Route("call/{contactId}/end/")]
        public IHttpActionResult PutEndCall(int contactId, JObject jsons)
        {
            var leadPersonId = jsons["managerId"];
            var occupiedId = jsons["occupiedId"];
            var initial = jsons["initial"];
            var url = jsons["url"];
            var actions =
                jsons["actions"].Select(
                    x => JsonConvert.DeserializeObject<ScriptAction>(x.ToString(), new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    })).ToList();

            _callService.GetProvider(CallTypes.Telesale).EndCall(contactId,
                leadPersonId != null ? leadPersonId.ToObject<int>() : 0,
                occupiedId != null ? occupiedId.ToObject<int>() : 0,
                initial != null ? initial.ToObject<string>() : null,
                url != null ? url.ToObject<string>() : null,
                actions);

            return Ok();
        }
    }
}