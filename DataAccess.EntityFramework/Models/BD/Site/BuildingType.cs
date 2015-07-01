using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.BD.Site
{
    [Table("BuildingType", Schema = "BD")]
    public class BuildingType
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }
        public string Type { get; set; }
        public string CriteriaDescription { get; set; }
        public virtual ICollection<BuildingQualifyCriteria> Criterias { get; set;  }  
    }

    [Table("BuildingQualifyCriteria", Schema = "BD")]
    public class BuildingQualifyCriteria
    {
        public BuildingQualifyCriteria()
        {
            From = null;
            To = null;
        }

        [Key]
        public int Id { get; set; }
        public string Size { get; set; }
        public double? From { get; set; }
        public double? To { get; set; }
        public bool AutoQualify { get; set; }
    }
}
