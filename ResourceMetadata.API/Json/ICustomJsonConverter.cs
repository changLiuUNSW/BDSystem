using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ResourceMetadata.API.Json
{
    interface ICustomJsonConverter
    {
        IContractResolver ContractResolver { get; set; }
        JsonSerializerSettings JsonSerializerSettings { get; set; }
        JObject ResolveObject(object value);
        JObject ResolveObject(object value, CustomJsonResolver resolver);
        JArray ResolveArray(object value);
        JArray ResolveArray(object value, CustomJsonResolver resolver);
    }
}
