using System.Collections.ObjectModel;
using DataAccess.Console.Scripts.Types;

namespace DataAccess.Console.Scripts
{
    internal class ScriptFactory : Collection<IScriptCreator>
    {
        public ScriptFactory()
        {
            Items.Add(new OprScriptCreator());
            Items.Add(new GovScriptCreator());
            Items.Add(new LpmScriptCreator());
            Items.Add(new BmsScriptCreator());
            Items.Add(new PmsScriptCreator());
            Items.Add(new BdScriptCreator());
        }
    }
}
