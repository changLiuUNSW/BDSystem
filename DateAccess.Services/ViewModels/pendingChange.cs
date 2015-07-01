using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EntityFramework.Models.BD.Contact;

namespace DateAccess.Services.ViewModels
{
    //pending change
    public class PendingChange
    {
        public ContactPerson Update { get; set; }
        public ContactPerson History { get; set; }
        public string Reason { get; set; }
        public string Name { get; set; }
    }
}
