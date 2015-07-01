using System.Collections.ObjectModel;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.ContactService.Call.Scripts.Data;

namespace DataAccess.Console.Scripts.Types
{
    /// <summary>
    /// provide common patterns for script creation
    /// </summary>
    internal abstract class ScriptCreator
    {
        /// <summary>
        /// this is the end of the script, it will always comes with update next call contact action
        /// </summary>
        /// <param name="updateInMonth"></param>
        /// <param name="actions"></param>
        /// <returns></returns>
        protected BinaryTreeNode<Script> End(bool updateInMonth, params ScriptAction[] actions)
        {
            var node = new BinaryTreeNode<Script>(new Script("Thank you for your time"))
            {
                Value =
                {
                    End = true,
                    Actions = new Collection<ScriptAction>
                    {
                        new UpdateNextCall(updateInMonth)
                    }
                }
            };

            foreach (var action in actions)
            {
                node.Value.Actions.Add(action);
            }

            return node;
        }
    }
}
