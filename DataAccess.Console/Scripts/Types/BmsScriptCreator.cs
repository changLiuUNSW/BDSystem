using System.Collections.ObjectModel;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;

namespace DataAccess.Console.Scripts.Types
{
    internal class BmsScriptCreator : ScriptCreator, IScriptCreator
    {
        public BinaryTree<Script> Create()
        {
            return Script();
        }

        private BinaryTree<Script> Script()
        {
            var tree = new BinaryTree<Script>(ScriptType.BMS.ToString());
            tree.Root = new BinaryTreeNode<Script>
            {
                Value = new Script
                {
                    BranchType = BranchTypes.ContctPerson.ToString(),
                    Branch = true,
                },
                Right = KnownContact(),
                Left = UnknownContact()
            };
            return tree;
        }

        private BinaryTreeNode<Script> KnownContact()
        {
            var root =
                new BinaryTreeNode<Script>(
                    new Script(
                        string.Format(
                            "Hello {0}, my name is {1} from Quad Services a locally based commercial cleaning, maintenance and security company. " +
                            "I am calling to introduce our company to see if I can arrange for our maintenance manager to come and see you and " +
                            "talk about our maintenance services. (building repairs, fit outs, plumbing, light replacement, painting",
                            Replaceable.String[ReplaceType.ContactName],
                            Replaceable.String[ReplaceType.TelesaleName]
                            )))
                {
                    Right = new BinaryTreeNode<Script>
                    {
                        Value = new Script("Confirm address, phone and location in building.")
                        {
                            Actions = new Collection<ScriptAction>
                            {
                                new CreateLead("Generate a new maintenance lead", ScriptActionType.CreateMaintenanceLead)
                            }
                        },
                        Left = new BinaryTreeNode<Script>
                        {
                            Value = new Script()
                            {
                                Branch = true,
                                BranchType = BranchTypes.PropertyMananger.ToString()
                            },
                            Left = UnknownPm(),
                            Right = KnownPm()
                        },
                        Right = End(true)
                    }
                };
            return root;
        }

        private BinaryTreeNode<Script> UnknownContact()
        {
            return null;
        }

        private BinaryTreeNode<Script> KnownPm()
        {
            return 
                new BinaryTreeNode<Script>(
                    new Script(string.Format("Is {0} still the property manager for your building?",
                        Replaceable.String[ReplaceType.PropertyManageName])))
                {
                    Left = UnknownPm(),
                    Right = AskForCleaning()
                };
        }

        private BinaryTreeNode<Script> UnknownPm()
        {
            return 
                new BinaryTreeNode<Script>(
                    new Script("For my records could you tell me who is the property manager for your building.")
                    {
                        Actions = new Collection<ScriptAction>
                        {
                            new UpdateContactName("Update contact name")
                        }
                    })
                {
                    Left = AskForCleaning(),
                    Right = AskForCleaning()
                };
        }

        private BinaryTreeNode<Script> AskForCleaning()
        {
            return 
                new BinaryTreeNode<Script>(
                    new Script("Thanks, by the way are you currently happy with your cleaning service?"))
                {
                    Left = End(true, new CreateTaskForDh()),
                    Right = End(true)
                };
        }
    }
}
