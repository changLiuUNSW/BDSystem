using System.Collections.ObjectModel;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;

namespace DataAccess.Console.Scripts.Types
{
    internal class LpmScriptCreator : ScriptCreator, IScriptCreator
    {
        public BinaryTree<Script> Create()
        {
            return Script();
        }

        private BinaryTree<Script> Script()
        {
            var tree = new BinaryTree<Script>(ScriptType.LPM.ToString())
            {
                Root = new BinaryTreeNode<Script>()
                {
                    Value = new Script
                    {
                        Branch = true,
                        BranchType = BranchTypes.ContctPerson.ToString()
                    },
                    Right = KnownContact(),
                    Left = UnknownContact()
                }
            };

            return tree;
        }

        private BinaryTreeNode<Script> KnownContact()
        {
            return 
                new BinaryTreeNode<Script>(
                    new Script(string.Format("Hello {0}, this is {1} from Quad, " +
                                             "we are a National Provider of cleaning services and are just calling to keep in touch. Is there anything we can help you with at the moment",
                        Replaceable.String[ReplaceType.ContactName],
                        Replaceable.String[ReplaceType.TelesaleName]))
                    {
                        Actions = new Collection<ScriptAction>
                        {
                            new UpdateNextCall(true),
                            new CreateTaskForDh()
                        }
                    })
                {
                    Right = new BinaryTreeNode<Script>()
                    {
                        Value = new Script(string.Format("I will arrange for {0} to contact you shortly.",
                            Replaceable.String[ReplaceType.QpName]))
                    },
                    Left = End(true)
                };
        }

        private BinaryTreeNode<Script> UnknownContact()
        {
            return new BinaryTreeNode<Script>
            {
                Value =
                    new Script(string.Format("Can I speak to Mr/Ms {0} please?",
                        Replaceable.String[ReplaceType.ContactName])),
                Left = End(false),
                Right = KnownContact()
            };
        }
    }
}
