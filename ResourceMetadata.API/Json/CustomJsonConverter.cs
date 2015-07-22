using System;
using System.Diagnostics.Contracts;
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

        /// <summary>
        /// default contact resolver for json serializer
        /// </summary>
        public IContractResolver ContractResolver {
            get { return _contractResolver ?? (_contractResolver = new DefaultContractResolver()); }
            set { _contractResolver = value; } 
        }

        /// <summary>
        /// default settings
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings { get; set; }

        public CustomJsonConverter()
        {
            
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public CustomJsonConverter(JsonSerializerSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            JsonSerializerSettings = settings;
        }

        /// <summary>
        /// default constructor with contract resolver
        /// </summary>
        /// <param name="resolver"></param>
        public CustomJsonConverter(IContractResolver resolver)
        {
            if (resolver == null)
                throw new ArgumentNullException("resolver");

            ContractResolver = resolver;
        }

        public CustomJsonConverter(IContractResolver resolver, JsonSerializerSettings settings)
        {
            if (resolver == null)
                throw new ArgumentNullException("resolver");

            if (settings == null)
                throw new ArgumentNullException("settings");

            ContractResolver = resolver;
            JsonSerializerSettings = settings;
        }

        /// <summary>
        /// resolve the object to JObject with the default resolver
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual JObject ResolveObject(object value)
        {
            return JObject.Parse(Serialize(value));
        }

        /// <summary>
        /// resolve an object model to JObject with provied resolver
        /// </summary>
        /// <param name="value"></param>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public virtual JObject ResolveObject(object value, CustomJsonResolver resolver)
        {
            ContractResolver = resolver;
            return ResolveObject(value);
        }

        /// <summary>
        /// resolve object to JArray with the default resolver
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual JArray ResolveArray(object value)
        {
            return JArray.Parse(Serialize(value));
        }

        /// <summary>
        /// resolve object to JArray with provided resolver
        /// </summary>
        /// <param name="value"></param>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public virtual JArray ResolveArray(object value, CustomJsonResolver resolver)
        {
            ContractResolver = resolver;
            return ResolveArray(value);
        }

        private string Serialize(object value)
        {
            JsonSerializerSettings.ContractResolver = ContractResolver;
            return JsonConvert.SerializeObject(value, JsonSerializerSettings); 
        }
    }
}