using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Providers;

namespace DateAccess.Services.ContactService.Call
{
    public interface ICallService
    {
        CallProvider GetProvider(CallTypes type);
        CallDetail GetNextCall(CallTypes type, string initial, int? lastCallId, int? siteId);
    }
}
