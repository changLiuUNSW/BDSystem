using System;
using System.Collections.ObjectModel;
using DataAccess.Console.Models;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.TypeLibrary;

namespace DataAccess.Console.Migration.DB.Mappers
{
    internal class CleaningContactMapper : ContactMapper
    {
        public CleaningContactMapper(MigrationConfiguration config) : base(config)
        {
        }

        public override object Map(COMPTEMP row)
        {

            if (row.CLEAN_CONT)
                return MapCleaningContact(row, true);

            if (row.SECU_CONT || row.MAINT_CONT)
                return null;

            //in the case of all three indicators are false
            //we still create a cleaning only if we can find a valid contact person
            var contactPerson = MapContactPerson(row);

            if (contactPerson == null)
                return null;

            var contact = MapCleaningContact(row, false);
            if (contact != null)
                contact.ContactPerson = contactPerson;

            return contact;
        }

        private Contact MapCleaningContact(COMPTEMP row, bool mapContactPerson)
        {
            var contact = base.Map(row) as Contact;
            if (contact == null)
                return null;

            contact.CallFrequency = row.CALL_ON_PM;
            contact.Note = row.NOTE_PAD;
            contact.NextCall = row.NEXT_CALL;
            contact.LastCall = row.LASTCONTAC;
            contact.BusinessTypeId = (int)BusinessTypes.Cleaning;
            
            if (mapContactPerson)
                contact.ContactPerson = MapContactPerson(row);

            return contact;
        }

        private ContactPerson MapContactPerson(COMPTEMP row)
        {
            if (string.IsNullOrEmpty(row.TITLE) &&
                string.IsNullOrEmpty(row.FIRST_NAME) &&
                string.IsNullOrEmpty(row.LAST_NAME) &&
                string.IsNullOrEmpty(row.EMAIL) &&
                string.IsNullOrEmpty(row.POSITION) &&
                string.IsNullOrEmpty(row.MOBILE) &&
                string.IsNullOrEmpty(row.FAX_CLEAN) &&
                string.IsNullOrEmpty(row.DIR_LINE) &&
                string.IsNullOrEmpty(row.POBOX) &&
                string.IsNullOrEmpty(row.POB_SUBURB) &&
                string.IsNullOrEmpty(row.POB_STATE) &&
                string.IsNullOrEmpty(row.POB_PCODE))
                return null;

            return new ContactPerson
            {
                Title = row.TITLE,
                Firstname = row.FIRST_NAME,
                Lastname = row.LAST_NAME,
                Email = row.EMAIL,
                Position = row.POSITION,
                Mobile = row.MOBILE,
                Fax = row.FAX_CLEAN,
                DirectLine = row.DIR_LINE,
                PoStreet = row.POBOX,
                PoSuburb = row.POB_SUBURB,
                PoState = row.POB_STATE,
                PoPostcode = row.POB_PCODE,
                CreateDate = DateTime.Today,
                Contacts = new Collection<Contact>(),
                Histories = new Collection<ContactPersonHistory>(),
            };
        }
    }
}
