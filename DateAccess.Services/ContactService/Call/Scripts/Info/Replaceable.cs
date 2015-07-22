using System;
using System.Collections.Generic;

namespace DateAccess.Services.ContactService.Call.Scripts.Info
{
    public enum ReplaceType
    {
        ContactName,
        ExtContactName,
        ManagerName,
        CallerName,
        SiteGroup,
        ContactTitle,
        AskQualification,
        AskOprQuestion,
        QpName,
        PropertyManageName,
        PropertyManageCompany,
        UpdateInterval,
        Quali_Question,
        NameCapture
    }

    public static class Replaceable
    {
        private static IDictionary<ReplaceType, string> _data;

        static Replaceable()
        {
            _data = new Dictionary<ReplaceType, string>();

            foreach (var name in Enum.GetNames(typeof(ReplaceType)))
            {
                _data.Add((ReplaceType)Enum.Parse(typeof(ReplaceType), name), AddMark(name));    
            }
        }

        public static IDictionary<ReplaceType, string> String
        {
            get { return _data; }
        }

        private static string AddMark(string target)
        {
            if (string.IsNullOrEmpty(target))
                return target;

            return '@' + target;
        }
    }
}
