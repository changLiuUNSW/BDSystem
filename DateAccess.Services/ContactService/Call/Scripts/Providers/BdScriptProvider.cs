using System;
using System.Text.RegularExpressions;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;
using DateAccess.Services.ContactService.Call.Scripts.Visitors.StringPattern;

namespace DateAccess.Services.ContactService.Call.Scripts.Providers
{
    public class BdScriptProvider : ScriptProvider
    {
        public Contact Contact { get; set; }
        public LeadPersonal LeadPerson { get; set; }

        public BdScriptProvider(Contact contact, LeadPersonal leadPerson)
        {
            Contact = contact;
            LeadPerson = leadPerson;
        }

        /// <summary>
        /// for BD we do not need to do special compiling on the questions nor do we care about the contact code
        /// </summary>
        /// <returns></returns>
        public override BinaryTree<Script> Get()
        {
            var script = GetScript(ScriptType.BD);

            if (script == null)
                return null;

            var pattern = new QuestionLookupPattern();
            var matches = pattern.Match(script.Root.Value.Question);

            foreach (Group match in matches)
            {
                var matchType = (ReplaceType) Enum.Parse(typeof (ReplaceType), match.Value.Remove(0, 1));

                switch (matchType)
                {
                    case ReplaceType.ContactName:

                        var toReplace = Contact != null ? Contact.ContactPerson != null ? Contact.ContactPerson.Lastname : "" : "";
                        script.Root.Value.Question = script.Root.Value.Question.Replace(match.Value, toReplace);
                        break;
                    case ReplaceType.CallerName:
                        script.Root.Value.Question = script.Root.Value.Question.Replace(match.Value, LeadPerson.Name);
                        break;
                }
            }

            return script;
        }
    }
}
