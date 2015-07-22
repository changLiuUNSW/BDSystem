using System;
using System.Linq;
using System.Text.RegularExpressions;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;
using DateAccess.Services.ContactService.Call.Scripts.Visitors.QuestionFilter;
using DateAccess.Services.ContactService.Call.Scripts.Visitors.StringPattern;

namespace DateAccess.Services.ContactService.Call.Scripts.Visitors
{
    /// <summary>
    /// visit each node and try to compile the information from contact/telelsale/leadperson to the script
    /// </summary>
    internal class QuestionCompilerVisitor : IVisitor
    {
        public Contact Contact { get; set; }
        public Telesale Telesale { get; set; }
        public LeadPersonal LeadPerson { get; set; }
        public IRegexPattern Pattern { get; set; }

        public QuestionCompilerVisitor(Contact contact, Telesale telesale, LeadPersonal leadPerson)
        {
            Contact = contact;
            Telesale = telesale;
            LeadPerson = leadPerson;
            Pattern = new QuestionLookupPattern();
        }

        public bool Visit<T>(T node)
        {
            var target = node as BinaryTreeNode<Script>;

            if (target == null)
                return true;

            TryCompileQuestion(target);
            if (target.Value.Actions != null && target.Value.Actions.Count > 0)
            {
                foreach (var action in target.Value.Actions)
                {
                    TryCompileAction(action);
                }
            }

            return true;
        }

        private void TryCompileAction(ScriptAction action)
        {
            var text = TryReplace(action.Description) as string;

            if (!string.IsNullOrEmpty(text))
                action.Description = text;
        }

        private void TryCompileQuestion(BinaryTreeNode<Script> node)
        {
            if (string.IsNullOrEmpty(node.Value.Question))
                return;

            var obj = TryReplace(node.Value.Question);
            var newNode = obj as BinaryTreeNode<Script>;
            if (newNode != null)
            {
                node.Value = newNode.Value;
                node.Left = newNode.Left;
                node.Right = newNode.Right;

                //if there is a new node, we need to translate that node
                TryCompileQuestion(node);
            }
            else
            {
                node.Value.Question = obj as string;
            }
        }

        private object TryReplace(string target)
        {
            var matches = Pattern.Match(target);
            foreach (Group match in matches)
            {
                var compiled = CompileNode((ReplaceType)Enum.Parse(typeof(ReplaceType), match.Value.Remove(0, 1)));
                var node = compiled as BinaryTreeNode<Script>;
                if (node != null)
                    return node;

                target = target.Replace(match.Value, compiled as string);
            }

            return target;
        }

        private object CompileNode(ReplaceType type)
        {
            switch (type)
            {
                case ReplaceType.ContactName:
                    return CompileContactName();
                case ReplaceType.ContactTitle:
                    return CompileContactTitle();
                case ReplaceType.CallerName:
                    return CompileCallerName();
                case ReplaceType.PropertyManageName:
                    return CompilePropertyManagerName();
                case ReplaceType.QpName:
                    return CompileQpName();
                case ReplaceType.UpdateInterval:
                    return CompileUpdateInterval();
                case ReplaceType.AskQualification:
                    return new QualificationFilter(Contact).GetNode();
                case ReplaceType.AskOprQuestion:
                    return new CleaningQuestionFilter(Contact).GetNode();
                case ReplaceType.NameCapture:
                    return Replaceable.String[ReplaceType.NameCapture];
                case ReplaceType.Quali_Question:
                    return CompileQualiQuestion();
                default:
                    return null;
            }
        }

        private string CompileContactName()
        {
            if (Contact == null || Contact.ContactPerson == null)
                return Replaceable.String[ReplaceType.ContactName];

            if (!string.IsNullOrEmpty(Contact.ContactPerson.Firstname) &&
                !string.IsNullOrEmpty(Contact.ContactPerson.Lastname))
                return string.Format("{0} {1}", Contact.ContactPerson.Firstname, Contact.ContactPerson.Lastname);

            if (!string.IsNullOrEmpty(Contact.ContactPerson.Firstname))
                return Contact.ContactPerson.Firstname;

            return Contact.ContactPerson.Lastname;
        }

        private string CompileContactTitle()
        {
            if (Contact == null || Contact.ContactPerson == null)
                return null;

            return Contact.ContactPerson.Title;
        }

        private string CompileCallerName()
        {
            if (Telesale == null)
                return null;

            return Telesale.Name;
        }

        private string CompilePropertyManagerName()
        {
            if (Contact == null || Contact.Site == null || Contact.Site.Groups == null)
                return null;

            var group = Contact.Site.Groups.SingleOrDefault(x=>x.Type.ToLower() == GroupType.Building.ToString().ToLower());
            if (group == null)
                return null;

            //todo how to determine the ext-manager 
            //return string.Format("{0}, {1}", group.Firstname, group.Lastname);
            throw new NotImplementedException();
        }

        private string CompileQpName()
        {
            var str = "David Hudson's team";
            if (Contact == null)
                return str;

            if (string.Compare(Contact.Code, "REA", StringComparison.InvariantCulture) == 0)
                str = LeadPerson.Name;

            return str;
        }

        private string CompileUpdateInterval()
        {
            if (Contact == null)
                return null;

            return Contact.CallFrequency.ToString();
        }

        private string CompileQualiQuestion()
        {
            if (Contact == null)
                return "no question found";

            return Contact.Site.BuildingType.CriteriaDescription;
        }
    }
}
