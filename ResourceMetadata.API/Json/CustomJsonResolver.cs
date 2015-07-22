using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ResourceMetadata.API.Json
{
    /// <summary>
    /// custom json conversion for contact
    /// </summary>
    public class CustomJsonResolver : DefaultContractResolver
    {
        /// <summary>
        /// list of ignore property when serializing json
        /// </summary>
        public IDictionary<Type, IList<string>> IgnoreList { get; set; }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);

            if (IgnoreList == null)
                throw new Exception("No ignore list supplied");

            Type matchedType = null;
            if (IgnoreList.ContainsKey(type))
                matchedType = type;
            else if (type.BaseType != null && IgnoreList.ContainsKey(type.BaseType))
                matchedType = type.BaseType;

            if (matchedType != null)
            {
                var list = IgnoreList[matchedType];
                foreach (var property in properties)
                {
                    if (list.Contains(property.PropertyName))
                    {
                        property.Ignored = true;
                    }        
                }
            }

            return properties;
        }
    }
}