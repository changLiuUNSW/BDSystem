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

        /// <summary>
        /// indicator for branch node, branch node does not have question
        /// </summary>
        public bool Branch { get; set; }

        public string BranchType { get; set; }

        /// <summary>
        /// actual question to be asked
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// this is to provide customized navigation text on script screen
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// indcaitor for the end of the node
        /// </summary>
        public bool End { get; set; }

        [XmlArrayItem(typeof(ScriptAction))]
        [XmlArrayItem(typeof(NewLead))]
        [XmlArrayItem(typeof(NewTaskForDh))]
        [XmlArrayItem(typeof(UpdateContactName))]
        [XmlArrayItem(typeof(UpdateQualification))]
        [XmlArrayItem(typeof(UpdateEmail))]
        [XmlArrayItem(typeof(UpdateExtManager))]
        [XmlArrayItem(typeof(UpdateDaCheck))]
        [XmlArrayItem(typeof(UpdateNextCall))]
        [XmlArrayItem(typeof(NewPropertyManager))]
        [XmlArrayItem(typeof(UpdateTenant))]
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto)]
        public Collection<ScriptAction> Actions { get; set; } 
    }
}