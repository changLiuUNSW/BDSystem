using DateAccess.Services.ContactService.Call.Models;

namespace DateAccess.Services.ContactService.Call.Providers
{
    /// <summary>
    /// standard call uses caller's initial to fetch the next contact
    /// </summary>
    public interface IStandardCall
    {
        CallDetail Next(string initial);
    }
}
