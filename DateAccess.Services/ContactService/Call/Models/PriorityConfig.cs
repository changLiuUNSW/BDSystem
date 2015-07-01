using System;
using DateAccess.Services.ContactService.Call.Scripts.Info;
using DateAccess.Services.ContactService.Call.Scripts.Serializers;

namespace DateAccess.Services.ContactService.Call.Models
{
    public class PriorityConfig
    {
        public string SuperAccount { get; set; }
        public string SuperAccountPriority { get; set; }
        public string SuperPriority { get; set; }

        public static PriorityConfig GetConfig()
        {
            var file = AppDomain.CurrentDomain.BaseDirectory + Constants.PConfig;
            return GetConfig(file);
        }

        public static PriorityConfig GetConfig(string file)
        {
            return Serializer.Deserialize<PriorityConfig>(file);
        }
    }
}
