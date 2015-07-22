using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.BD.Site
{
    [Table("SiteGroup", Schema = "BD")]
    public class SiteGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string GroupName { get; set; }
        public string AgentComp { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }

        public virtual ICollection<ExternalManager> ExternalManagers { get; set; } 
        public virtual ICollection<Site> Sites { get; set; } 
    }
}
