using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EntityFramework.Models.BD.Contact;

namespace DateAccess.Services.ContactService.Call.Exceptions
{
    public class InvalidContactIdException : Exception
    {
        public int ContactId { get; set; }

        public InvalidContactIdException(int id): base(string.Format("Contact Id {0} not found in the system", id))
        {
            ContactId = id;
        }

        public InvalidContactIdException(int id, string message) : base(message)
        {
            ContactId = id;
        }
    }
}
