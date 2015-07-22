using System;
using System.Collections.ObjectModel;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;
using DateAccess.Services.ContactService.Call.Scripts.Visitors;
using DateAccess.Services.ContactService.Call.Scripts.Visitors.TravelPattern;

namespace DateAccess.Services.ContactService.Call.Scripts.Providers
{
    public class TelesaleScriptProvider : ScriptProvider
    {
        public Contact Contact { get; set; }
        public Telesale Telesale { get; set; }
        public LeadPersonal LeadPerson { get; set; }

        public TelesaleScriptProvider(Contact contact, Telesale telesale, LeadPersonal leadPerson)
        {
            Contact = contact;
            Telesale = telesale;
            LeadPerson = leadPerson;
        }

        public override BinaryTree<Script> Get()
        {
            if (Contact == null)
                return null;

            ScriptType type;
            switch (Contact.Code)
            {
                case Constants.Government:
                case Constants.Group:
                    type = ScriptType.GRP;
                    break;
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
    }
}
