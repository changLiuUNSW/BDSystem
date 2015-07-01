using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using DataAccess.EntityFramework.Models.BD.Contact;
using DateAccess.Services.ContactService;
using DateAccess.Services.ViewModels;

namespace ResourceMetadata.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/contactperson")]
    [Authorize]
    public class ContactPersonController : ApiController
    {
        private readonly IContactPersonService _contactPersonService; 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactPersonService"></param>
        public ContactPersonController(IContactPersonService contactPersonService)
        {
            _contactPersonService = contactPersonService;
        }

        /// <summary>
        /// api/contactperson/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var contactPerson = _contactPersonService.GetByKey(id);
            if (contactPerson == null)
                return NotFound();

            var contactPersonDto = Mapper.Map<ContactPersonDTO>(contactPerson);
            return Ok(new
            {
                data = contactPersonDto,
            });
        }

        /// <summary>
        /// update contact person to contact
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        [Route("contact")]
        [HttpPut]
        public IHttpActionResult UpdatePersonForContact(List<UpdatePersonModel> contacts)
        {
            _contactPersonService.UpdatePersonForContact(contacts);
            return Ok();
        }

        /// <summary>
        /// create a new contact person
        /// </summary>
        /// <param name="contactPerson"></param>
        /// <returns></returns>
        public IHttpActionResult PostContactPerson(ContactPerson contactPerson)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            contactPerson.CreateDate = DateTime.Today;
            var addedPerson = _contactPersonService.Add(contactPerson);
            var contactPersonDto = Mapper.Map<ContactPersonDTO>(addedPerson);
            return Ok(new
            {
                data = contactPersonDto
            });
        }

        /// <summary>
        /// update contact person
        /// </summary>
        /// <param name="contactPerson"></param>
        /// <returns></returns>
        public IHttpActionResult PutContactPerson(ContactPerson contactPerson)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _contactPersonService.Update(contactPerson);
            var contactPersonDto = Mapper.Map<ContactPersonDTO>(contactPerson);
            return Ok(new
            {
                data = contactPersonDto
            });
        }

        /// <summary>
        /// delete the contact person
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult DeleteContactPerson(int id)
        {
            _contactPersonService.Delete(id);
            return Ok();
        }

        /// <summary>
        /// get update history of the contact person
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="personId"></param>
        /// <returns></returns>
        [Route("history/{personId}")]
        public IHttpActionResult GetPersonHistory(string reason=null, [FromUri]int? personId = null)
        {
            var history = _contactPersonService.GetChangeHistory(reason, personId);
            return Ok(new
            {
                data=history
            });
        }

        /// <summary>
        /// delete update history of the contact person
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Route("history")]
        [HttpDelete]
        public IHttpActionResult DeletePersonHistory([FromUri]int[] ids )
        {
            _contactPersonService.RemovePersonHistories(ids);
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Route("history/newmanager")]
        [HttpPost]
        public IHttpActionResult HistoryChangeToNewManager([FromBody]List<int> ids)
        {
            _contactPersonService.HistoryChangeToNewManager(ids);
            return Ok(new
            {
                data = ids
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("history")]
        public IHttpActionResult GetAllNameCorrectHistory()
        {
            var historyList = _contactPersonService.GetNameCorrectHistoryList();
            var historyListDto=Mapper.Map<IList<ContactPersonHistoryDTO>>(historyList);
            return Ok(new
            {
                data = historyListDto
            });
        }

        /// <summary>
        /// update contact person at the same time move the original contact person to the history table
        /// </summary>
        /// <returns></returns>
        [Route("PendingUpdate")]
        public IHttpActionResult PutPending([FromBody]IList<PendingChange> changes)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _contactPersonService.UpdatePendingChanges(changes);
            return Ok();
        }
    }
}