using System.Linq;
using DataAccess.EntityFramework.Models.BD.Contact;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;

namespace DateAccess.Services.ContactService.Call.Scripts.Visitors.QuestionFilter
{
    internal class CleaningQuestionFilter : Filter
    {
        public CleaningQuestionFilter(Contact contact) : base(contact) { }

        public override BinaryTreeNode<Script> GetNode()
        {
            Template = GetTemplateFromFile();
            if (Template == null || Contact == null)
                return null;

            BinaryTree<Script> node;

            if (Contact.Site.Groups != null && Contact.Site.Groups.Any(x => x.Type == "QT" || x.Type == "QA"))
            {
                node = Template.Scripts.SingleOrDefault(x => x.Tag == ScriptType.Cln_QaQt.ToString());
            }
            else if (Contact.Site.InHouse)
            {
                node = Template.Scripts.SingleOrDefault(x => x.Tag == ScriptType.Cln_Inhouse.ToString());
            }
            else if (Contact.Site.SalesBox.Security)
            {
                node = Template.Scripts.SingleOrDefault(x => x.Tag == ScriptType.Cln_Security.ToString());
            }
            else
            {
                node = Template.Scripts.SingleOrDefault(x => x.Tag == ScriptType.Cln_Only.ToString());
            }

            if (node != null)
                return node.Root;

            return null;
        }
    }
}
