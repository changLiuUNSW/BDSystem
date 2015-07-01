using System.Linq;
using DataAccess.EntityFramework.Extensions.Utilities;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.TypeLibrary;

namespace DataAccess.EntityFramework.Extensions
{
    public static class BusinessTypeExtensions
    {
        public static BusinessType Get(this IQueryable<BusinessType> source, BusinessTypes type)
        {
            var str = type.GetDescription();;
            return source.Single(x => x.Type == str);
        }
    }
}
