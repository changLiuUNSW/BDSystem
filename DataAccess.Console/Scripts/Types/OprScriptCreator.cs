using System.Collections.ObjectModel;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;

namespace DataAccess.Console.Scripts.Types
{
    internal class OprScriptCreator : ScriptCreator, IScriptCreator 
    {
        public BinaryTree<Script> Create()
        {
            return Script();
        }

        private BinaryTree<Script> Script()
        {
            var tree = new BinaryTree<Script>(ScriptType.OPR.ToString())
            {
                Root = new BinaryTreeNode<Script>
                {
                    Value = new Script()
                    {
                        BranchType = BranchTypes.ContctPerson.ToString(),
                        Branch = true
                    },

                    Right = KnownContact(), 
                    Left = UnknowContact()
                }
            };

            return tree;
        }

        private BinaryTreeNode<Script> KnownContact()
        {
            var root = new BinaryTreeNode<Script>(new Script
            {
                Question = string.Format("Can I speak to {0} please?", Replaceable.String[ReplaceType.ContactName])
            });

            root.Left = new BinaryTreeNode<Script>(new Script
            {
                Question = "We would like to offer a free quotation for cleaning services.",
            });

            root.Left.Left = End(false);

            root.Right = new BinaryTreeNode<Script> { Value = new Script(Replaceable.String[ReplaceType.AskOprQuestion]) };
            root.Left.Right = new BinaryTreeNode<Script> { Value = new Script(Replaceable.String[ReplaceType.AskOprQuestion]) };
            return root;
        }

        private BinaryTreeNode<Script> UnknowContact()
        {
            var root = new BinaryTreeNode<Script>(new Script
            {
                Question =
                    "Could you please tell me the name of the person who is responsible for the cleaning of your premises?",
                Actions = new Collection<ScriptAction>
                {
                    new UpdateContactName("Update contact name")
                },
            });

            root.Right = KnownContact();
            root.Left = new BinaryTreeNode<Script>(new Script
            {
                Question =
                    "We would like to send the relevant person some information on our services that they could find useful in the future. I do not " +
                    "need to speak to the person. Could you just tell me their name",
                Actions = new Collection<ScriptAction>
                {
                    new UpdateContactName("Update contact name")
                }
            });

            root.Left.Left = End(false);
            root.Left.Right = End(true);

            return root;
        }
    }
}
