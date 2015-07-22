using System.Collections.ObjectModel;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;

namespace DataAccess.Console.Scripts.Types
{
    internal class PmsScriptCreator :ScriptCreator, IScriptCreator
    {
        public BinaryTree<Script> Create()
        {
            return Script();
        }

        private BinaryTree<Script> Script()
        {
            var tree = new BinaryTree<Script>(ScriptType.PMS.ToString())
            {
                Root = new BinaryTreeNode<Script>
                {
                    Value = new Script
                    {
                        Branch  = true,
                        BranchType = BranchTypes.PropertyMananger.ToString()
                    },
                    Left = NotPm(),
                    Right = IsPm()
                }
            };

            return tree;
        }

        private BinaryTreeNode<Script> IsPm()
        {
            var left = new BinaryTreeNode<Script>
                {
                    Value = new Script("Would you give me the name and phone number of the new property manager?")
                    {
                        Actions = new Collection<ScriptAction>
                        {
                            new NewPropertyManager()
                        }
                    },
                    Left = new BinaryTreeNode<Script>
                    {
                        Value =
                            new Script(
                                "Can you put me through to someone in your office - office manager / accountant who would know?"),
                        Left = End(true),
                        Right = new BinaryTreeNode<Script>
                        {
                            Value =
                                new Script(
                                    "Can you give me the name and phone number of the property manager of this building?")
                                {
                                    Actions = new Collection<ScriptAction>()
                                    {
                                        new NewPropertyManager()
                                    }
                                },
                            Right = End(true),
                            Left = End(false)
                        }
                    },
                    Right = End(true)
                };


            return new BinaryTreeNode<Script>
            {
                Value =
                    new Script(
                        string.Format(
                            "Good morning, this is {0} from Quad Services. I would like to confirm that {1} " +
                            "is the company that manages this property", Replaceable.String[ReplaceType.CallerName],
                            Replaceable.String[ReplaceType.PropertyManageCompany])),
                Left = left,
                Right = new BinaryTreeNode<Script>
                {
                    Value = new Script(string.Format("Is {0} the property manager?",
                    Replaceable.String[ReplaceType.PropertyManageName])),
                    Right = End(true),
                    Left = left
                }
            };
        }

        private BinaryTreeNode<Script> NotPm()
        {
            return new BinaryTreeNode<Script>
            {
                Value = new Script(string.Format("Good morning, this is {0} from Quad Services. " +
                                                 "Could you please tell me if your company engages the cleaners or whether the cleaning is part of your rental.",
                    Replaceable.String[ReplaceType.CallerName])),
                Right = new BinaryTreeNode<Script>
                {
                    Value = new Script
                    {
                        Question =
                            "Could you please tell me the name of the person who is responsible for organising the cleaning of your premises?",
                        Actions = new Collection<ScriptAction>
                        {
                            new UpdateTenant()
                        },
                        Text = "Internal"
                    },
                    Right = new BinaryTreeNode<Script>
                    {
                        Value =
                            new Script(string.Format("Can I speak to Mr/Ms {0} please?",
                                Replaceable.String[ReplaceType.NameCapture])),
                        Right = new BinaryTreeNode<Script>
                        {
                            Value =
                                new Script(
                                    string.Format(
                                        "Hello, My name is {0} from Quad Services a locally based commercial cleaning company, I am calling to see if I can arrange for" +
                                        " our manager to provide you with a quotation for your cleaning services.",
                                        Replaceable.String[ReplaceType.CallerName])),
                            Left = End(true),
                            Right = new BinaryTreeNode<Script>
                            {
                                Value = new Script("Confirm detail")
                                {
                                    Actions = new Collection<ScriptAction>
                                    {
                                        new NewLead("New cleaning lead", ScriptActionType.CreateCleaningLead)
                                    },
                                },
                                Right = new BinaryTreeNode<Script>
                                {
                                    Value = new Script(string.Format("{0} will be contacting you shortly to arrange to call in and see you.", Replaceable.String[ReplaceType.QpName]))
                                    {
                                        End = true,
                                    }
                                },
                                Left = End(true)
                            }
                        },
                        Left = End(false)

                    },
                    Left = End(false)
                },
                Left = new BinaryTreeNode<Script>
                {
                    Value =
                        new Script(
                            "Could you please tell me the name of the property manager who organises the cleaning of your premises? (also company / phone")
                        {
                            Actions = new Collection<ScriptAction> {new NewPropertyManager()},
                            Text = "External"
                        },
                    Right = End(true),
                    Left = new BinaryTreeNode<Script>
                    {
                        Value = new Script("Can you transfer me to someone who could assist me?"),
                        Left = End(false),
                        Right = new BinaryTreeNode<Script>
                        {
                            Value =
                                new Script(
                                    "Could you please tell me the name of the property manager who organises the cleaning of your premises? (also company / phone")
                                {
                                    Actions = new Collection<ScriptAction> {new NewPropertyManager()}
                                },
                            Left = End(false),
                            Right = new BinaryTreeNode<Script>
                            {
                                Value = new Script("Confirm detail")
                                {
                                    Actions = new Collection<ScriptAction>
                                    {
                                        new NewPropertyManager()
                                    }
                                },
                                Left = End(true),
                                Right = End(true)
                            }

                        }
                    }
                }
            };
        }
    }
}
