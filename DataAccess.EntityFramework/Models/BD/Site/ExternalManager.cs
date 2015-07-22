using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.BD.Site
{
    [Table("ExternalManager", Schema = "BD")]
    public class ExternalManager
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }

        public int GroupId { get; set; }
        public virtual SiteGroup Group { get; set; }
    }
}
