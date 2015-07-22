using System.Collections.Generic;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Providers;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.MailService;

namespace DateAccess.Services.ContactService.Call
{
    internal class CallService : ICallService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailHelper _emailHelper;
        private readonly IContactService _contactService;

        public CallService(IUnitOfWork unitOfWork, IEmailHelper emailHelper, IContactService contactService)
        {
            _unitOfWork = unitOfWork;
            _emailHelper = emailHelper;
            _contactService = contactService;
        }

        /// <summary>
        /// return next call base on supplied informations
        /// </summary>
        /// <param name="type">type of call provider</param>
        /// <param name="initial">initial for find next call, ignored if siteId is supplied</param>
        /// <param name="siteId">siteId for find next call</param>
        /// <param name="lastCallId">Qccupied call id</param>
        /// <returns></returns>
        public CallDetail GetNextCall(CallType type, string initial, int? siteId, int? lastCallId)
        {
            var provider = GetProvider(type);
            if (lastCallId.HasValue)
                _contactService.RemoveOccupiedCall(lastCallId.Value);

            if (siteId.HasValue)
                return provider.GroupCall.Next(initial, siteId.Value);

            if (!string.IsNullOrEmpty(initial))
                return provider.StandardCall.Next(initial);

            return null;
        }

        public CallProvider GetProvider(CallType type)
        {
            return CallProvider.Create(type, _unitOfWork, _emailHelper);
        }
    }
}
