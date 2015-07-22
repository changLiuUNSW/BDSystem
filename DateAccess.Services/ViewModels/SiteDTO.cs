using System.Collections.Generic;
using DataAccess.EntityFramework.Models.BD.Site;

namespace DateAccess.Services.ViewModels
{
    public class SiteDTO
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public List<ContactDTO> Contacts { get; set; }
        public List<ContactPersonDTO> ContactPersons { get; set; }

        public List<SiteGroupDTO> Groups { get; set; }

        public CleaningContractDTO CleaningContract { get; set; }
        public SecurityContractDTO SecurityContract { get; set; }

        public string GmZone { get; set; }

        public string Region { get; set; }

        public string Number { get; set; }

        public string Unit { get; set; }

        public string Street { get; set; }

        public string Suburb { get; set; }

        public string State { get; set; }

        public string Postcode { get; set; }

        public string Name { get; set; }

        public bool PropertyManaged { get; set; }

        public bool PMSite { get; set; }

        public BuildingType BuildingType { get; set; }

        public string Phone { get; set; }

        public string Size { get; set; }

        public bool InHouse { get; set; }
    }
}