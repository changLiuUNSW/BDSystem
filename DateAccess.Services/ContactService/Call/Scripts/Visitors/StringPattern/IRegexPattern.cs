using System.Text.RegularExpressions;

namespace DateAccess.Services.ContactService.Call.Scripts.Visitors.StringPattern
{
    public interface IRegexPattern
    {
        MatchCollection Match(string target);
    }
}
