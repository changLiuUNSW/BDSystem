using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DataAccess.EntityFramework.Models.BD.Lead
{
    [Table("LeadStatus", Schema = "BD")]
    public class LeadStatus
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Order { get; set; }
        public bool Hidden { get; set; }

        [NotMapped]
        public int Count
        {
            get
            {
                return Leads == null ? 0 : Leads.Count;
            }
        }

        [JsonIgnore]
        public virtual ICollection<Lead> Leads { get; set; }
    }
}
