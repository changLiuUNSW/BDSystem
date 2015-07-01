using System;
using System.Collections.Generic;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;

namespace ResourceMetadata.API.Json
{
    /// <summary>
    /// json serialization for contact from telesale call
    /// </summary>
    public class TelesaleContactJsonConverter : CustomJsonConverter
    {
        /// <summary>
        /// list of ignore property when serializing json
        /// </summary>
        public IDictionary<Type, IList<string>> IgnoreList { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public TelesaleContactJsonConverter()
        {
            IgnoreList = new Dictionary<Type, IList<string>>
            {
                //site related type
                //{typeof (Site), new[] {"ContactPersons"}},
                {typeof (BuildingType), new [] {"Criterias"}},
                {typeof (CleaningContract), new[] {"Site"}},
                {typeof (SecurityContract), new[] {"Site"}},

                //contact related type
                {typeof (Contact), new[] {"Leads", "Site", "BusinessType"}},
                {typeof (ContactPerson), new []{"Site", "Contacts", "Histories"}},
                {typeof (CallLine), new[] {"Contact"}},

                //leadPerson related type
                {
                    typeof (LeadPersonal),
                    new[]
                    {
                        "GroupId", "LeadGroup", "Allocation", "Quotes", "Leads", "LeadStop", "LeadTarget", "TempLeadStop",
                        "TempLeadTarget", "PriorityToCall", "LeadsOnHoldDate", "LeadsOnHoldReason"
                    }
                }
            };

            var resolver = new CustomJsonResolver {IgnoreList = IgnoreList};
            JsonSerializerSettings.ContractResolver = resolver;
        }
    }
}