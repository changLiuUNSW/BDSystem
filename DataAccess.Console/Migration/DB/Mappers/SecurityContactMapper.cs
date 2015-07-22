using System;
using System.Collections.ObjectModel;
using DataAccess.Console.Models;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.TypeLibrary;

namespace DataAccess.Console.Migration.DB.Mappers
{
    internal class SecurityContactMapper : ContactMapper
    {
        public SecurityContactMapper(MigrationConfiguration config) : base(config) { }

        public override object Map(COMPTEMP row)
        {
            if (!row.SECU_CONT)
                return null;

            var contact = base.Map(row) as Contact;

            if (contact == null)
                return null;

            contact.CallFrequency = row.SE_CALLCYC;
            contact.Note = row.SE_CT_MEMO;
            contact.NextCall = row.SENEXTCALL;
            contact.LastCall = row.SELASTCALL;
            contact.BusinessTypeId = (int) BusinessTypes.Security;
            contact.ContactPerson = MapContactPerson(row);
            return contact;
        }

        public ContactPerson MapContactPerson(COMPTEMP row)
        {
            if (string.IsNullOrEmpty(row.SE_TITLE) &&
                string.IsNullOrEmpty(row.SE_F_NAME) &&
                string.IsNullOrEmpty(row.SE_L_NAME) &&
                string.IsNullOrEmpty(row.SE_EMAIL) &&
                string.IsNullOrEmpty(row.SE_POST) &&
                string.IsNullOrEmpty(row.SE_DIRLINE))
                return null;

            return new ContactPerson
            {
                Title = row.SE_TITLE,
                Firstname = row.SE_F_NAME,
                Lastname = row.SE_L_NAME,
                Email = row.SE_EMAIL,
                Position = row.SE_POST,
                DirectLine = row.SE_DIRLINE,
                CreateDate = DateTime.Today,
                Contacts = new Collection<Contact>(),
                Histories = new Collection<ContactPersonHistory>()
                //did not find a field for secruity fax
                //Fax =
            };
        }
    }
}
