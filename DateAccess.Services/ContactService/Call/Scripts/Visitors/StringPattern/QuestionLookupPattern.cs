using System;
using System.Text.RegularExpressions;

namespace DateAccess.Services.ContactService.Call.Scripts.Visitors.StringPattern
{
    internal class QuestionLookupPattern : IRegexPattern
    {
        public string Pattern { get; set; }

        public QuestionLookupPattern()
        {
            Pattern = @"\@\w*";
        }

        public MatchCollection Match(string target)
        {
            return Regex.Matches(target, Pattern, RegexOptions.IgnoreCase);
        }
    }
}
