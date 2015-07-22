using System;
using System.Linq.Expressions;
using DataAccess.Common.SearchModels;
using DataAccess.EntityFramework.Models.BD.Contact;

namespace DateAccess.Services.SearchService.Projector
{
    public class AdminContactProjector : IQueryProjector
    {
        public Expression<Func<Contact, AdminSearch>> Expression
        {
            get
            {
                return x => new AdminSearch
                {
                    BusinessType = x.BusinessType.Type,
                    ContactPersonId = x.ContactPersonId,
                    Key = x.Site.Key,
                    SalesRep = x.Code,
                    Company = x.Site.Name,
                    FirstName = x.ContactPerson.Firstname,
                    LastName = x.ContactPerson.Lastname,
                    Position = x.ContactPerson.Position,
                    Mobile = x.ContactPerson.Mobile,
                    DirLine = x.ContactPerson.DirectLine,
                    Email = x.ContactPerson.Email,
                    NextCall = x.NextCall,
                    LastCall = x.LastCall
                };
            }
        }

        public Expression<Func<T, TResult>> CreateExpression<T, TResult>() where T : class where TResult : class
        {
            return (Expression<Func<T, TResult>>)Convert.ChangeType(Expression, typeof(Expression<Func<T, TResult>>));
        }
    }
}
