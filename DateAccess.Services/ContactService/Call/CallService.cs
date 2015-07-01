using System;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Providers;
using DateAccess.Services.MailService;

namespace DateAccess.Services.ContactService.Call
{
    internal class CallService : ICallService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILeadEmailService _emailService;
        private readonly IContactService _contactService;

        public CallService(IUnitOfWork unitOfWork, ILeadEmailService emailService, IContactService contactService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _contactService = contactService;
        }

        public CallProvider Provider { get; set; } 
        public CallProvider GetProvider(CallTypes type)
        {
            switch (type)
            {
                case CallTypes.Telesale:
                    return new TelesaleCallProvider(_unitOfWork, _emailService);
                case CallTypes.BD:
                    return new BdCallProvider(_unitOfWork, _emailService);
                default:
                    return null;
            }
        }

        /// <summary>
        /// return next call base on supplied informations
        /// </summary>
        /// <param name="type">type of call provider</param>
        /// <param name="initial">initial for find next call, ignored if siteId is supplied</param>
        /// <param name="siteId">siteId for find next call</param>
        /// <param name="lastCallId">Qccupied call id</param>
        /// <returns></returns>
        public CallDetail GetNextCall(CallTypes type, string initial, int? siteId, int? lastCallId)
        {
            var provider = Provider ?? GetProvider(type);

            if (lastCallId.HasValue)
                _contactService.RemoveOccupiedCall(lastCallId.Value);

            if (siteId.HasValue)
                return provider.GroupCall.Next(siteId.Value);

            if (!string.IsNullOrEmpty(initial))
                return provider.StandardCall.Next(initial);

            return null;
        }
    }
}
