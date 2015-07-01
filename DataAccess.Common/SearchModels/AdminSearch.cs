using System;
using System.Collections.Generic;

namespace DataAccess.Common.SearchModels
{
    public class AdminSearch
    {
        public int? ContactId { get; set; }
        public int? SiteId { get; set; }
        public int? ContactPersonId { get; set; }
        public string Key { get; set; }
        //Site Info
        public string Company { get; set; }
        public string Unit { get; set; }
        public string Number { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Suburb { get; set; }
        public string BuildingName { get; set; }
        public string BuildingType { get; set; }

        //Contact Person Info
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }

        public string DirLine { get; set; }
        public string Email { get; set; }

        //Contact
        public string SalesRep { get; set; }

        public IEnumerable<string> SalesRepList { get; set; }

        public string BusinessType { get; set; }
        public bool? DaToCheck { get; set; }
        public string InfoToCheck { get; set; }
        public DateTime? NextCall { get; set; }
        public DateTime? LastCall { get; set; }
        //ContractType
    }
}