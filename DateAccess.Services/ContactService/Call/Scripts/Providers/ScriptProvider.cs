using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;
using DateAccess.Services.ContactService.Call.Scripts.Serializers;

namespace DateAccess.Services.ContactService.Call.Scripts.Providers
{
    /// <summary>
    /// base provider is to provide basic script access and serialization functionalities
    /// </summary>
    public abstract class ScriptProvider
    {
        protected ScriptProvider() { }

        public static IList<BinaryTree<Script>> GetScripts()
        {
            var file = GetFile();
            if (file == null || file.Scripts == null)
                return null;

            return file.Scripts;
        }

        public static BinaryTree<Script> GetScript(ScriptType type)
        {
            var file = GetFile();

            if (file == null || file.Scripts == null)
                return null;

            if (file.Scripts.Count <= 0)
                return null;

            return  file.Scripts.SingleOrDefault(x => (ScriptType) Enum.Parse(typeof (ScriptType), x.Tag, true) == type);
        }

        public static ScriptXmlTemplate GetFile(string file = null)
        {
            string path;

            if (string.IsNullOrEmpty(file))
                path = AppDomain.CurrentDomain.BaseDirectory + Constants.ScriptLocation;
            else
                path = file;


            if (!File.Exists(path))
                return null;

            return Serializer.Deserialize<ScriptXmlTemplate>(path);
        }

        public abstract BinaryTree<Script> Get();
    }
}
