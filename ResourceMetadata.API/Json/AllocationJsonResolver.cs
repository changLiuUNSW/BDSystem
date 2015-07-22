using System;
using System.Collections.Generic;
using DataAccess.EntityFramework.Models.BD.Lead;

namespace ResourceMetadata.API.Json
{
    /// <summary>
    /// custome json resolver to eliminate tables that are not needed or are heavily linked
    /// use for allocation table  
    /// </summary>
    public class AllocationJsonResolver : CustomJsonResolver
    {
        public AllocationJsonResolver()
        {
            IgnoreList = new Dictionary<Type, IList<string>>
            {
                {typeof (LeadPersonal), new[]
                {
                    "Allocations", 
                    "Quotes", 
                    "Leads", 
                    "OccupiedContacts",
                }}
            };
        }
    }
}