using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using DateAccess.Services.ContactService.Call.Scripts.Data;

namespace DateAccess.Services.ContactService.Call.Models
{
    /// <summary>
    /// serializable object for storing all scripts in the file
    /// </summary>
    [XmlRoot]
    [Serializable]
    public class ScriptXmlTemplate
    {
        [XmlArrayItem(ElementName = "Script")]
        public Collection<BinaryTree<Script>> Scripts { get; set; }

        public ScriptXmlTemplate()
        {
            Scripts = new Collection<BinaryTree<Script>>();
        }

        public ScriptXmlTemplate(Collection<BinaryTree<Script>> scripts)
        {
            Scripts = scripts;
        }
    }
}
