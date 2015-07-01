using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.EntityFramework.Expressions;
using DataAccess.EntityFramework.Models.BD.Allocation;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.TypeLibrary;

namespace DataAccess.EntityFramework.Extensions
{
    internal static class ContactExtensions
    {
        /// <summary>
        /// put high value contacts(contact that has had a new manager in the last 12 months) on top of the call queque in query result in query result
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IOrderedQueryable<Contact> OrderByNewManager(this IQueryable<Contact> source)
        {
            return source.OrderBy(x => x.NewManagerDate < DbFunctions.AddMonths(DateTime.Today.Date, -12) || x.NewManagerDate == null);
        }

        /// <summary>
        /// then by version of OrderByNewManager in query result
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IOrderedQueryable<Contact> ThenByNewManager(
            this IOrderedQueryable<Contact> source)
        {
            return source.ThenBy(x => x.NewManagerDate < DbFunctions.AddMonths(DateTime.Today.Date, -12) || x.NewManagerDate == null);
        }

        public static IOrderedQueryable<Contact> ArrangeCallOrder(this IQueryable<Contact> source)
        {
            return source.OrderByNewManager()
                .ThenBy(x => x.Site.InHouse)
                .ThenByDescending(x => x.Site.Size)
                .ThenBy(x => x.NextCall);
        }

        public static IQueryable<Contact> CleaningContacts(this IQueryable<Contact> source)
        {
            var result = source.Where(x => x.BusinessTypeId == (int)BusinessTypes.Cleaning);
            return result;
        }

        /// <summary>
        /// return clean contacts base on provided parmeters
        /// </summary>
        /// <param name="source"></param>
        /// <param name="qpCodes">included qp codes</param>
        /// <returns></returns>
        public static IQueryable<Contact> CleaningContacts(this IQueryable<Contact> source, IEnumerable<string> qpCodes)
        {
            var result = source.CleaningContacts().Where(x => qpCodes.Contains(x.Code));
            return result;
        }

        public static IQueryable<Contact> ReadyToCall(this IQueryable<Contact> source, bool isTelesale = true)
        {
            return
                source.Where(
                    x =>
                        x.NextCall != null &&
                        x.NextCall <= DateTime.Today &&
                        x.ExtManagement == false &&
                        x.DaToCheck == false &&
                        (x.CallBackDate <= DateTime.Today || x.CallBackDate == null) &&
                        x.CallFrequency > 0 &&
                        x.Site.TsToCall == isTelesale &&
                        x.Leads.All(y => y.LeadStatusId == (int) LeadStatusTypes.Cancelled ||
                                         y.LeadStatusId == (int) LeadStatusTypes.Quoted));
        } 

        public static IQueryable<Contact> FilterByAllocation(this IQueryable<Contact> source, IEnumerable<Allocation> allocations)
        {
            Expression<Func<Contact, bool>> expression = null;
            foreach (var allocation in allocations)
            {
                var size = allocation.Size;
                var zone = allocation.Zone;
                Expression<Func<Contact, bool>> expr = x => x.Site.Size == size && x.Site.SalesBox.Zone == zone;

                if (expression == null)
                    expression = expr;
                else
                    expression = expression.Or(expr);
            }

            if (expression != null)
            {
                return source.Where(expression);
            }

            return source;
        }
    }
}
