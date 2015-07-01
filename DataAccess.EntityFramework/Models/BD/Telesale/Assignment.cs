using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DataAccess.EntityFramework.Models.BD.Telesale
{
    //telesale assignment for making calls
    [Table("TelesaleAssignment", Schema = "BD")]
    public class Assignment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int Id { get; set; }

        [Key, Column(Order = 1)]
        public string QpCode { get; set; }

        [Key, Column(Order = 2)]
        public string Area { get; set; }
        
        public string Size { get; set; }

        [ForeignKey("Telesale")]
        public int TelesaleId { get; set; }

        [JsonIgnore]
        public virtual Telesale Telesale { get; set; }
    };
}
