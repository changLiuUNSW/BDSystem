using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ResourceMetadata.API.Json
{
    /// <summary>
    /// provide custom json serialization
    /// </summary>
    public class CustomJsonConverter : ICustomJsonConverter
    {
        private IContractResolver _contractResolver;
        private JsonSerializerSettings _jsonSerializerSettings;

        /// <summary>
        /// default contact resolver for json serializer
        /// </summary>
        public IContractResolver ContactResolver {
            get {
                if (_contractResolver == null)
                {
                    _contractResolver = new DefaultContractResolver();   
                }

                return _contractResolver;
            }
            set { _contractResolver = value; } 
        }

        /// <summary>
        /// default settings
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings {
            get
            {
                if (_jsonSerializerSettings == null)
                {
                    _jsonSerializerSettings = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        DateTimeZoneHandling = DateTimeZoneHandling.Local,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        ContractResolver = ContactResolver
                    };
                }

                return _jsonSerializerSettings;
            }
            set { _jsonSerializerSettings = value; } 
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public CustomJsonConverter()
        {
        }

        /// <summary>
        /// default constructor with contract resolver
        /// </summary>
        /// <param name="resolver"></param>
        public CustomJsonConverter(IContractResolver resolver)
        {
            if (resolver == null)
                throw new Exception("Invalid json resolver");

            ContactResolver = resolver;
        }

        /// <summary>
        /// resolve the object using set contract resolver and jsonSerializerSettings to JObject
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual JObject Resolve(object value)
        {
            return JObject.Parse(JsonConvert.SerializeObject(value, JsonSerializerSettings));
        }

    }
}