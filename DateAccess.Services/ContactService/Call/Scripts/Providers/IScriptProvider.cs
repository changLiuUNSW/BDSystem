using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Data;

namespace DateAccess.Services.ContactService.Call.Scripts.Providers
{
    public interface IScriptProvider
    {
        Contact Contact { get; set; }
        Telesale Telesale { get; set; }
        LeadPersonal LeadPerson { get; set; }

        BinaryTree<Script> Get();
    }
}
