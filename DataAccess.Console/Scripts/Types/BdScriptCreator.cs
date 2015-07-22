using System;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;

namespace DataAccess.Console.Scripts.Types
{
    public class BdScriptCreator : IScriptCreator
    {
        public BinaryTree<Script> Create()
        {
            return Script();
        }

        public BinaryTree<Script> Script()
        {
            var tree = new BinaryTree<Script>(ScriptType.BD.ToString())
            {
                Root = new BinaryTreeNode<Script>
                {
                    Value = new Script
                    {
                        End = true,
                        Branch = false,
                        Question = string.Format("This is {0} calling from Quad - just to keeping in touch. " +
                                                 "Is there anything I can help you with at the moment ...... " +
                                                 "OK I will give you a quick all in a few month \r\n" +
                                                 "\r\n" +
                                                 "Hello {1}, {2} from Quad calling - just a quick call to touch base and see " +
                                                 "how your cleaning is going. \r\n" +
                                                 "\r\n" +
                                                 "We want to call each three months. We want to keep it very short so they do " +
                                                 "not mind the next call. If we hard sell each call we produce the problem that they " +
                                                 "do not want to take the regular call! We chat and spend time when the client wants to " +
                                                 "chat (relationship devloping) but otherwise we keep it short as above.",
                                                 Replaceable.String[ReplaceType.CallerName], 
                                                 Replaceable.String[ReplaceType.ContactName],
                                                 Replaceable.String[ReplaceType.CallerName])
                    }
                }
            };

            return tree;
        }
    }
}
