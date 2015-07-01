using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;
using DateAccess.Services.ContactService.Call.Scripts.Visitors.TravelPattern;

namespace DateAccess.Services.ContactService.Call.Scripts.Visitors.QuestionFilter
{
    internal class QualificationFilter : Filter
    {
        public QualificationFilter(Contact contact) : base (contact) { }

        public override BinaryTreeNode<Script> GetNode()
        {
            Template = GetTemplateFromFile();
            if (Template == null || Contact == null)
                return null;

            //only need to ask two questions
            var firstTwo = GetQuestions().Take(2).ToList();
            if (firstTwo.Count == 2)
            {
                var visitors = new Collection<IVisitor> {new QualificationCompilerVisitor(firstTwo[1])};
                firstTwo[0].Traverse(new PreOrderTravel(), visitors);
                return firstTwo[0];
            }

            return firstTwo.FirstOrDefault();
        }

        private bool IsQualified()
        {
            if (Contact == null)
                return true;

            var criteria = Contact.Site.BuildingType.Criterias.SingleOrDefault(x => x.Size == Contact.Site.Size);

            //if criteria does not exist in the database, auto qualify
            if (criteria == null)
                return true;

            if (criteria.AutoQualify)
                return true;

            if (Contact.Site.Qualification.HasValue == false)
                return false;

            if (criteria.From < Contact.Site.Qualification.Value)
                return true;

            return false;
        }

        private bool ValidBuildingType()
        {
            if (Contact == null || Contact.Site == null)
                return false;

            if (Contact.Site.BuildTypeId.HasValue == false)
                return false;

            return true;
        }

        private IEnumerable<BinaryTreeNode<Script>> GetQuestions()
        {
            if (ValidBuildingType() &&
                !IsQualified() &&
                !string.IsNullOrEmpty(Contact.Site.BuildingType.CriteriaDescription))
            {
                var tree = Template.Scripts.SingleOrDefault(x => x.Tag == ScriptType.Quali_Question.ToString());

                if (tree != null)
                    yield return tree.Root;
            }

            if (Contact.ContactPersonId != null &&
                Contact.ContactPerson != null &&
                string.IsNullOrEmpty(Contact.ContactPerson.Email))
            {
                var tree = Template.Scripts.SingleOrDefault(x => x.Tag == ScriptType.Quali_Email.ToString());
                if (tree != null)
                    yield return tree.Root;
            }

            if (Contact.Site.Contacts.All(x => x.BusinessTypeId != (int) BusinessTypes.Security))
            {
                var tree = Template.Scripts.SingleOrDefault(x => x.Tag == ScriptType.Quali_Security.ToString());
                if (tree != null)
                    yield return tree.Root;
            }

            if (Contact.Site.SalesBox.Maintenance)
            {
                var tree = Template.Scripts.SingleOrDefault(x => x.Tag == ScriptType.Quali_Maintenance.ToString());
                if (tree != null)
                    yield return tree.Root;
            }
        }
    };
}
