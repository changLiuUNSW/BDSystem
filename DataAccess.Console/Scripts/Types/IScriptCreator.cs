using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Data;

namespace DataAccess.Console.Scripts.Types
{
    internal interface IScriptCreator
    {
        BinaryTree<Script> Create();
    }
}
