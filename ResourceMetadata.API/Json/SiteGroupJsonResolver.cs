using System;
using System.Collections.Generic;
using DataAccess.EntityFramework.Models.BD.Site;

namespace ResourceMetadata.API.Json
{
    /// <summary>
    /// manually reoslving the site group into json
    /// </summary>
    public class SiteGroupJsonResolver : CustomJsonResolver
    {
        public SiteGroupJsonResolver()
        {
            IgnoreList = new Dictionary<Type, IList<string>>
            {
                {typeof (ExternalManager), new[] {"SiteGroup"}},
                {
                    typeof (Site),
                    new[]
                    {
                        "Contacts", "ContactPersons", "Groups", "CleaningContract", "SecurityContract", "SalesBox",
                        "BuildingType"
                    }
                }
            };
        }
    }
}