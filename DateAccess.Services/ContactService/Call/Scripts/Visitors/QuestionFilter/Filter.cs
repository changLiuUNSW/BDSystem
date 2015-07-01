using System;
using System.IO;
using DataAccess.EntityFramework.Models.BD.Contact;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;
using DateAccess.Services.ContactService.Call.Scripts.Serializers;

namespace DateAccess.Services.ContactService.Call.Scripts.Visitors.QuestionFilter
{
    internal abstract class Filter
    {
        public Contact Contact { get; set; }
        public ScriptXmlTemplate Template { get; set; }

        protected Filter(Contact contact)
        {
            Contact = contact;
        }

        protected virtual ScriptXmlTemplate GetTemplateFromFile()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + Constants.ScriptLocation;
            if (!File.Exists(path))
                return null;

            Template = Serializer.Deserialize<ScriptXmlTemplate>(path);
            return Template;
        }

        public abstract BinaryTreeNode<Script> GetNode();

    }
}
