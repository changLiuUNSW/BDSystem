using System;

namespace DataAccess.Common.SearchModels
{
    public class LeadSearch
    {
        //Lead
        public int Id { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LeadType { get; set; }
        public string Status { get; set; }
        public bool Hidden { get; set; }
        public string Phone { get; set; }

        //Site
        public string SiteName { get; set; }
        public string SiteUnit { get; set; }
        public string SiteNumber { get; set; }
        public string SiteStreet { get; set; }
        public string SiteSuburb { get; set; }
        public string SiteState { get; set; }
        public string SitePostcode { get; set; }
       
       
        //Contact Person
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //QP
        public string QpInitial { get; set; }
        public string QpName { get; set; }
    }
}