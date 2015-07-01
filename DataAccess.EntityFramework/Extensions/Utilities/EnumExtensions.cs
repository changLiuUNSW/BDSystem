using System.ComponentModel;
using DataAccess.EntityFramework.TypeLibrary;

namespace DataAccess.EntityFramework.Extensions.Utilities
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Size source)
        {
            return GetDescription((object) source);
        }

        public static string GetDescription(this BusinessTypes source)
        {
            return GetDescription((object) source);
        }

        public static bool Has(this LeadGroups source, LeadGroups target)
        {
            var result = source & target;
            return (target == result);
        }

        private static string GetDescription(object obj)
        {
            var type = obj.GetType();

            var members = type.GetMember(obj.ToString());

            if (members.Length > 0)
            {
                var attrs = members[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return obj.ToString();
        }
    }
}
