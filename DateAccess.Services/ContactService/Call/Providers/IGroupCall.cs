using DateAccess.Services.ContactService.Call.Models;

namespace DateAccess.Services.ContactService.Call.Providers
{
    /// <summary>
    /// group call is site based, need to provide siteId in order to fetch the call
    /// it does not involve any call queues like standard call do
    /// </summary>
    public interface IGroupCall
    {
        CallDetail Next(string name, int siteId);
    }
}
