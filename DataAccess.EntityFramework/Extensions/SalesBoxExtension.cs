using System.Collections.Generic;
using System.Linq;
using DataAccess.EntityFramework.Models.BD.Allocation;

namespace DataAccess.EntityFramework.Extensions
{
    internal static class SalesBoxExtension
    {
        /// <summary>
        /// convert zones to post codes and states
        /// </summary>
        /// <param name="source"></param>
        /// <param name="zoneList"></param>
        /// <param name="postCodes"></param>
        /// <param name="states"></param>
        public static void GetPostCodesAndStates(this IQueryable<SalesBox> source, 
            ICollection<string> zoneList, 
            out IList<string> postCodes, 
            out IList<string> states)
        {
            var zones = source.Where(x => zoneList.Contains(x.Zone));
            postCodes = zones.Select(x => x.Postcode).ToList();
            states = zones.Select(x => x.State).ToList();
        }

        /// <summary>
        /// convert single zone to post codes and states
        /// this function is done in memory instead of from database
        /// </summary>
        /// <param name="source"></param>
        /// <param name="zone"></param>
        /// <param name="postCodes"></param>
        /// <param name="states"></param>
        public static void GetPostCodesAndStates(this IList<SalesBox> source, 
            string zone, 
            out IList<string> postCodes, 
            out IList<string> states )
        {
            var saleboxes = source.Where(x => x.Zone == zone).ToList();
            postCodes = saleboxes.Select(x => x.Postcode).Distinct().ToList();
            states = saleboxes.Select(x => x.State).Distinct().ToList();
        }
    }
}
