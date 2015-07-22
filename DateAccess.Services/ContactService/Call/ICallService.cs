using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Providers;

namespace DateAccess.Services.ContactService.Call
{
    public interface ICallService
    {
        CallDetail GetNextCall(CallType type, string initial, int? lastCallId, int? siteId);
        CallProvider GetProvider(CallType type);
    }
}
