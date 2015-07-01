using System.Collections.ObjectModel;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.ContactService.Call.Scripts.Data;

namespace DataAccess.Console.Scripts.Types
{
    internal abstract class CleaningScriptCreator : ScriptCreator
    {
        protected BinaryTreeNode<Script> Accept()
        {
            return new BinaryTreeNode<Script>
            {
                Value = new Script
                {
                    Question = "I would like to confirm your address and phone number.",
                    Actions = new Collection<ScriptAction>
                    {
                        new CreateLead("Generate a new lead.", ScriptActionType.CreateCleaningLead)
                    }
                },

                Left = End(false),
                Right = End(true)
            };
        }
    }
}
