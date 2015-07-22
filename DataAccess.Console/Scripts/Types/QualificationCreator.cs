using System.Collections.Generic;
using System.Collections.ObjectModel;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;

namespace DataAccess.Console.Scripts.Types
{
    public class QualificationCreator
    {
        internal ICollection<IScriptCreator> Creators { get; set; } 

        public QualificationCreator()
        {
            Creators = new Collection<IScriptCreator>
            {
                new QualifierQuestion(),
                new EmailQuestion(),
                new SecurityQuestion(),
                new MaintenanceQuestion()
            };
        }

        private class QualifierQuestion : ScriptCreator, IScriptCreator
        {
            public BinaryTree<Script> Create()
            {
                var tree = new BinaryTree<Script>(ScriptType.Quali_Question.ToString())
                {
                    Root = new BinaryTreeNode<Script>
                    {
                        Value = new Script
                        {
                            Question = "Ask for " + Replaceable.String[ReplaceType.Quali_Question],
                            Actions = new Collection<ScriptAction>()
                            {
                                new UpdateQualification("Update qualification number")
                            }
                        },

                        Left = End(true),
                        Right = End(true)
                    }
                };
                return tree;
            }
        }

        private class EmailQuestion : ScriptCreator, IScriptCreator
        {
            public BinaryTree<Script> Create()
            {
                var tree = new BinaryTree<Script>(ScriptType.Quali_Email.ToString())
                {
                    Root = new BinaryTreeNode<Script>
                    {
                        Value = new Script
                        {
                            Question = "Is it okay if we can send you our company profile by email?"
                        },
                        Right = new BinaryTreeNode<Script>
                        {
                            Value = new Script
                            {
                                Question = "Could I get your email address?",
                                Actions = new Collection<ScriptAction>
                                {
                                    new UpdateEmail()
                                }
                            },
                            Right = End(true),
                            Left = End(true)
                        },
                        Left = End(true)
                    }
                };

                return tree;
            }
        }

        private class SecurityQuestion : ScriptCreator, IScriptCreator
        {
            public BinaryTree<Script> Create()
            {
                var tree = new BinaryTree<Script>(ScriptType.Quali_Security.ToString())
                {
                    Root = new BinaryTreeNode<Script>
                    {
                        Value = new Script
                        {
                            Question =
                                "Our organisation also provide security services. Could you please tell me the name of the person who is reponsible for the security of your premises?"
                        },
                        Left = End(true),
                        Right = End(true)
                    }
                };

                return tree;
            }
        }

        private class MaintenanceQuestion : ScriptCreator, IScriptCreator
        {
            public BinaryTree<Script> Create()
            {
                var tree = new BinaryTree<Script>(ScriptType.Quali_Maintenance.ToString())
                {
                    Root = new BinaryTreeNode<Script>
                    {
                        Value = new Script
                        {
                            Question =
                                "We also have a maintenance division that can provide painting, plumbing, office alterations, handyman and other maintenance servics. " +
                                "Would you be interested in speaking to our maintenance manager further regarding any of these services?"

                        },
                        Right = new BinaryTreeNode<Script>
                        {
                            Value = new Script
                            {
                                Question = "I would like to confirm your address and phone number.",
                                Actions = new Collection<ScriptAction>
                                {
                                    new NewLead("New maintenance lead.",
                                        ScriptActionType.CreateMaintenanceLead)
                                }
                            },
                            Right = new BinaryTreeNode<Script>
                            {
                                Value = new Script
                                {
                                    Question = "Tony Jeffs will contact you shortly",
                                    Actions = new Collection<ScriptAction>
                                    {
                                        new UpdateNextCall()
                                    },
                                    End = true
                                }
                            },
                            Left = End(true)
                        },
                        Left = End(true)
                    }
                };

                return tree;
            }
        }
    }
}
