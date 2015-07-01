using System.Collections.ObjectModel;
using System.Xml.Serialization;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using Newtonsoft.Json;

namespace DateAccess.Services.ContactService.Call.Models
{
    public class Script
    {
        //[XmlAttribute]
        //public bool Choice { get; set; }

        public Script()
        {
            
        }

        public Script(string question)
        {
            Question = question;
        }

        public bool Branch { get; set; }
        public string BranchType { get; set; }
        public string Question { get; set; }
        public string Text { get; set; }
        public bool End { get; set; }

        [XmlArrayItem(typeof(ScriptAction))]
        [XmlArrayItem(typeof(CreateLead))]
        [XmlArrayItem(typeof(CreateTaskForDh))]
        [XmlArrayItem(typeof(UpdateContactName))]
        [XmlArrayItem(typeof(UpdateQualification))]
        [XmlArrayItem(typeof(UpdateEmail))]
        [XmlArrayItem(typeof(UpdateExtManager))]
        [XmlArrayItem(typeof(UpdateDaCheck))]
        [XmlArrayItem(typeof(UpdateNextCall))]
        [XmlArrayItem(typeof(UpdatePropertyManager))]
        [XmlArrayItem(typeof(UpdateTenant))]
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto)]
        public Collection<ScriptAction> Actions { get; set; } 
    }
}