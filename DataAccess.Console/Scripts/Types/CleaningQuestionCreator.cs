using System.Collections.Generic;
using System.Collections.ObjectModel;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;

namespace DataAccess.Console.Scripts.Types
{
    internal class CleaningQuestionCreator
    {
        internal ICollection<IScriptCreator> Creators { get; set; }

        public CleaningQuestionCreator()
        {
            Creators = new Collection<IScriptCreator>
            {
                new CleaningQaQt(),
                new CleaningInhouse(),
                new CleaningAndSecurity(),
                new CleaningOnly()
            };
        }

        private class CleaningQaQt : CleaningScriptCreator, IScriptCreator
        {
            public BinaryTree<Script> Create()
            {
                var tree = new BinaryTree<Script>(ScriptType.Cln_QaQt.ToString())
                {
                    Root = new BinaryTreeNode<Script>
                    {
                        Value = new Script
                        {
                            Question = string.Format(
                                "Hello {0}, my name is {1} from Quad Services a locally based commercial cleaning company. Currently we provide cleaning service to {2}." +
                                " I am calling to see if I can arrange for our manager to provide you with a quotation for your cleaning services.",
                                Replaceable.String[ReplaceType.ContactName],
                                Replaceable.String[ReplaceType.CallerName],
                                Replaceable.String[ReplaceType.SiteGroup]),
                        },
                        Right = Accept(),
                        Left = new BinaryTreeNode<Script>
                        {
                            Value = new Script
                            {
                                Question =
                                    "This would be an obligation free quotation and we would endeavour to not take up much of your time."
                            },
                            Left = new BinaryTreeNode<Script>
                            {
                                Value = new Script(Replaceable.String[ReplaceType.AskQualification]),
                            },
                            Right = Accept()
                        }
                    }
                };

                return tree;
            }
        }

        private class CleaningInhouse : CleaningScriptCreator, IScriptCreator
        {
            public BinaryTree<Script> Create()
            {
                var tree = new BinaryTree<Script>(ScriptType.Cln_Inhouse.ToString())
                {
                    Root = new BinaryTreeNode<Script>
                    {
                        Value = new Script
                        {
                            Question =
                                string.Format(
                                    "Hello {0}, my name is {1} from Quad Services a locally based commercial cleaning company. " +
                                    "I am calling to see if our manager can be of assistance with your cleaning requirements",
                                    Replaceable.String[ReplaceType.ContactName],
                                    Replaceable.String[ReplaceType.CallerName]),
                        },
                        Right = Accept(),
                        Left = new BinaryTreeNode<Script>
                        {
                            Value = new Script
                            {
                                Question =
                                    "Can I just confirm that you still employ your own cleaning staff directly, or do you now use outside."
                            },
                            Left = new BinaryTreeNode<Script>
                            {
                                Value = new Script(Replaceable.String[ReplaceType.AskQualification])
                            },
                            Right = Accept()
                        }
                    }
                };

                return tree;
            }
        };

        private class CleaningAndSecurity : CleaningScriptCreator, IScriptCreator
        {
            public BinaryTree<Script> Create()
            {
                var tree = new BinaryTree<Script>(ScriptType.Cln_Security.ToString())
                {
                    Root = new BinaryTreeNode<Script>
                    {
                        Value = new Script
                        {
                            Question =
                                string.Format(
                                    "Hello {0}, my name is {1} from Quad Services a locally based commercial cleaning & security company, " +
                                    "I am calling to see if I can arrange for our manager to provide you with a quotation for your cleaning and security services",
                                    Replaceable.String[ReplaceType.ContactName],
                                    Replaceable.String[ReplaceType.CallerName])
                        },

                        Right = Accept(),
                        Left = new BinaryTreeNode<Script>
                        {
                            Value =
                                new Script
                                {
                                    Question =
                                        "This would be an obligation free quotation and we would endeavour to not take up much of your time."
                                },
                            Left = new BinaryTreeNode<Script>
                            {
                                Value = new Script(Replaceable.String[ReplaceType.AskQualification])
                            },
                            Right = Accept()
                        }
                    }
                };

                return tree;
            }
        }

        private class CleaningOnly : CleaningScriptCreator, IScriptCreator
        {
            public BinaryTree<Script> Create()
            {
                var tree = new BinaryTree<Script>(ScriptType.Cln_Only.ToString())
                {
                    Root = new BinaryTreeNode<Script>
                    {
                        Value = new Script
                        {
                            Question =
                                string.Format(
                                    "Hello {0}, my name is {1} from Quad Services a locally based commercial cleaning company, " +
                                    "I am calling to see if I can arrange for our manager to provide you with a quotation for your cleaning services",
                                    Replaceable.String[ReplaceType.ContactName],
                                    Replaceable.String[ReplaceType.CallerName]),
                        },

                        Right = Accept(),
                        Left = new BinaryTreeNode<Script>
                        {
                            Value =
                                new Script
                                {
                                    Question =
                                        "This would be an obligation free quotation and we would endeavour to not take up much of your time."
                                },
                            Left = new BinaryTreeNode<Script>
                            {
                                Value = new Script(Replaceable.String[ReplaceType.AskQualification]),
                            },
                            Right = Accept()
                        }
                    }
                };

                return tree;
            }
        }
    }
}
