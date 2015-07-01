using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;
using DateAccess.Services.ContactService.Call.Scripts.Serializers;
using DateAccess.Services.ContactService.Call.Scripts.Visitors;
using DateAccess.Services.ContactService.Call.Scripts.Visitors.TravelPattern;

namespace DateAccess.Services.ContactService.Call.Scripts.Providers
{
    public class ScriptProvider : IScriptProvider
    {
        public Contact Contact { get; set; }
        public Telesale Telesale { get; set; }
        public LeadPersonal LeadPerson { get; set; }

        public ScriptProvider(Contact contact, Telesale telesale, LeadPersonal leadPerson)
        {
            Contact = contact;
            Telesale = telesale;
            LeadPerson = leadPerson;
        }

        public BinaryTree<Script> Get()
        {
            if (Contact == null)
                return null;

            ScriptType type;

            switch (Contact.Code)
            {
                case Constants.Government:
                case Constants.Group:
                    return null;
                case Constants.LPM:
                case Constants.REA:
                    type = ScriptType.LPM;
                    break;
                default:
                    var canParse = Enum.TryParse(Contact.Code, true, out type);

                    if (!canParse)
                        return null;

                    break;
            }

            return PrepareScript(GetScript(type));
        }

        private BinaryTree<Script> PrepareScript(BinaryTree<Script> script)
        {
            if (script == null)
                return null;

            var visitors = new Collection<IVisitor>
            {
                new PathCompilerVisitor(Contact),
                new QuestionCompilerVisitor(Contact, Telesale, LeadPerson)
            };

            script.Traverse(new PreOrderTravel(), visitors);
            return script;
        }

        /// <summary>
        /// find the script within the file according to contact type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private BinaryTree<Script> GetScript(ScriptType type)
        {
            var file = GetFile();

            if (file == null || file.Scripts == null)
                return null;

            if (file.Scripts.Count <= 0)
                return null;

            return file.Scripts.SingleOrDefault(x => (ScriptType) Enum.Parse(typeof (ScriptType), x.Tag, true) == type);
        }

        /// <summary>
        /// deserialize xml file into the object
        /// </summary>
        /// <returns></returns>
        private ScriptXmlTemplate GetFile()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + Constants.ScriptLocation;

            if (!File.Exists(path))
                return null;

            return Serializer.Deserialize<ScriptXmlTemplate>(path);
        }
    }
}
