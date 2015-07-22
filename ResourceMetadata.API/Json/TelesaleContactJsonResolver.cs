using System;
using System.Collections.Generic;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;

namespace ResourceMetadata.API.Json
{
    /// <summary>
    /// rules for resolving contact for telesale module
    /// </summary>
    public class TelesaleContactJsonResolver : CustomJsonResolver
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TelesaleContactJsonResolver()
        {
            IgnoreList = new Dictionary<Type, IList<string>>
            {
                //site related type
                //{typeof (Site), new[] {"ContactPersons"}},
                {typeof (BuildingType), new [] {"Criterias"}},
                {typeof (CleaningContract), new[] {"Site"}},
                {typeof (SecurityContract), new[] {"Site"}},
                {typeof (ExternalManager), new [] {"Group"}},

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
        }
    }
}