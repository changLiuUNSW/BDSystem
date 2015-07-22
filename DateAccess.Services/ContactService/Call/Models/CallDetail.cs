using System.Collections.Generic;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using Newtonsoft.Json;

namespace DateAccess.Services.ContactService.Call.Models
{
    public class CallDetail
    {
        public int OccupiedId { get; set; }
        
        /// <summary>
        /// name of person who gets the lead
        /// </summary>
        public LeadPersonal LeadPerson { get; set; }
        
        /// <summary>
        /// actual contact information for calling
        /// </summary>
        public Contact Contact { get; set; }

        /// <summary>
        /// site for contacts
        /// </summary>
        public Site Site { get; set; }
       
        /// <summary>
        /// the script used for telesale
        /// </summary>
        public BinaryTree<Script> Script { get; set; } 
        
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto)]
        public IList<ScriptAction> ScriptActions { get; set; }
    }
}
