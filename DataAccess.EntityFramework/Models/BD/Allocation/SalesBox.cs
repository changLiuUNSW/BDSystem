using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.BD.Allocation
{
    [Table("SalesBox", Schema = "BD")]
    public class SalesBox
    {
        [Key,Column(Order = 0)]
        public string Postcode { get; set; }
        [Key,Column(Order = 1)]
        public string State { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
        public bool Security { get; set; }
        public bool Maintenance { get; set; }
    }
}
