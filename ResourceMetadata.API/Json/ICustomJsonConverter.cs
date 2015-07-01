using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ResourceMetadata.API.Json
{
    interface ICustomJsonConverter
    {
        IContractResolver ContactResolver { get; set; }
        JsonSerializerSettings JsonSerializerSettings { get; set; }
        JObject Resolve(object value);
    }
}
