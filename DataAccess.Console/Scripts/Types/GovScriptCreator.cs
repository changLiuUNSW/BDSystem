using System.Collections.ObjectModel;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;

namespace DataAccess.Console.Scripts.Types
{
    internal class GovScriptCreator : ScriptCreator, IScriptCreator
    {
        public BinaryTree<Script> Create()
        {
            return Script();
        }

        private BinaryTree<Script> Script()
        {
            var tree = new BinaryTree<Script>(ScriptType.GRP.ToString());

            tree.Root = new BinaryTreeNode<Script>
            {
                Value = new Script
                {
                    Branch = true,
                    BranchType = BranchTypes.ContctPerson.ToString()
                },
                Right = KnownContact(),
                Left = UnknownContact()
            };
            return tree;
        }

        private BinaryTreeNode<Script> KnownContact()
        {
            return
                new BinaryTreeNode<Script>(
                    new Script(string.Format("Can I speak to {0} please?", Replaceable.String[ReplaceType.ContactName])))
                {
                    Left = new BinaryTreeNode<Script>
                    {
                        Value = new Script("We would like to offer a free quotation for cleaning services."),
                        Left = End(false),
                        Right = GetQuestionList()
                    },
                    Right = GetQuestionList()
                };
        }

        private BinaryTreeNode<Script> UnknownContact()
        {
            return
                new BinaryTreeNode<Script>
                {
                    Value = new Script(
                        "Could you please tell me the name of the person who is responsible for the cleaning of your premises.")
                    {
                        Actions = new Collection<ScriptAction>
                        {
                            new UpdateContactName()
                        }
                    },
                    Right = KnownContact(),
                    Left = new BinaryTreeNode<Script>
                    {
                        Value = new Script(
                            "We would like to send the relevant person some information on our services that they chould find useful in the future. " +
                            "I do not need to speak to the person. Could you just tell me their name")
                        {
                            Actions = new Collection<ScriptAction>
                            {
                                new UpdateContactName()
                            }
                        },
                        Left = End(false),
                        Right = End(true)
                    }
                };
        }

        private BinaryTreeNode<Script> GetQuestionList()
        {
            return 
                new BinaryTreeNode<Script>(
                    new Script(string.Format("Hello {0} {1}, My name is {2} from Quad Services. " +
                                             "I am calling to check whether your cleaning procurement is still managed externally " +
                                             "- not by someone in your office - or whether you select the contractor yourselves.",
                        Replaceable.String[ReplaceType.ContactTitle],
                        Replaceable.String[ReplaceType.ContactName],
                        Replaceable.String[ReplaceType.CallerName])))
                {
                    Right = new BinaryTreeNode<Script>
                    {
                        Value = new Script
                        {
                            Branch = true,
                            BranchType = BranchTypes.PropertyMananger.ToString()
                        },
                        Left = UnknownExternalContact(),
                        Right = KnownExternalContact()
                    },
                    Left = new BinaryTreeNode<Script>
                    {
                        Value = new Script(
                            "Quad Services is a locally based commercial cleaning company, I am calling to see if I can arrange for our manager to provide " +
                            "you with a quotation for your cleaning services.")
                        {
                            Text = "Internal"
                        },
                        Left = new BinaryTreeNode<Script>
                        {
                            Value = new Script(
                                "This would be an obligation free quotation and we would endeavour to not take up much of your time"),
                            Right = Accept(),
                            Left = new BinaryTreeNode<Script>
                            {
                                Value = new Script(Replaceable.String[ReplaceType.AskQualification])
                            }
                        },
                        Right = Accept()
                    }
                };
        }

        private BinaryTreeNode<Script> UnknownExternalContact()
        {
            return new BinaryTreeNode<Script>
            {
                Value = new Script(
                    "Can you inform me of the contact and phone number of the external person who handles the cleaning?")
                {
                    Text = "External",
                    Actions = new Collection<ScriptAction>
                    {
                        new UpdateContactName("Update contact name")
                    }
                },
                Right = End(true),
                Left = new BinaryTreeNode<Script>
                {
                    Value = new Script("Can you transfer me to someone who can tell me?"),
                    Right = new BinaryTreeNode<Script>
                    {
                        Value =
                            new Script(
                                "Can you inform me of the contact and phone number of the external person who handles the cleaning?")
                            {
                                Actions = new Collection<ScriptAction>
                                {
                                    new UpdateContactName("Update contact name")
                                }
                            },
                            Left = End(false),
                            Right = End(true)
                    },
                    Left = End(false)
                }
            };
        }

        private BinaryTreeNode<Script> KnownExternalContact()
        {
            return new BinaryTreeNode<Script>(new Script(
                string.Format(
                    "Would you confirm that the contact details for the external person who manages the cleaning is {0}",
                    Replaceable.String[ReplaceType.ExtContactName]))
            {
                Text = "Internal"
            })
            {
                Right = End(true),
                Left = new BinaryTreeNode<Script>
                {
                    Value = new Script("Can you transfer me to someone who can tell me?"),
                    Right = new BinaryTreeNode<Script>
                    {
                        Value = new Script(
                            string.Format(
                                "Would you confirm that the contact details for the external person who manages the cleaning is {0}",
                                Replaceable.String[ReplaceType.ExtContactName])),

                        Left = End(false),
                        Right = End(true)
                    },
                    Left = End(false)
                }
            };
        }

        private BinaryTreeNode<Script> Accept()
        {
            return new BinaryTreeNode<Script>(new Script("I would like to confirm your address and phone number.")
            {
                Actions = new Collection<ScriptAction>
                {
                    new NewLead("New cleaning lead", ScriptActionType.CreateCleaningLead)
                }
            })
            {
                Left = End(true),
                Right = new BinaryTreeNode<Script>
                {
                    Value =
                        new Script(string.Format("{0} will be contacting you shortly to arrange to call in.",
                            Replaceable.String[ReplaceType.QpName]))
                        {
                            Actions = new Collection<ScriptAction>
                            {
                                new UpdateNextCall(true),
                            },
                            End = true
                        }
                }
            };
        }
    }
}
