using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.EntityFramework.Models.BD.Allocation;
using DataAccess.EntityFramework.Models.Quote.Cost;
using Newtonsoft.Json;

namespace DataAccess.EntityFramework.Models.BD.Site
{
    /// <summary>
    /// Temp SITE table
    /// </summary>
    [Table("TempSite", Schema = "BD")]
    public class TempSite
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }

        [MaxLength(25)]
        [ForeignKey("SalesBox")]
        [Column(Order = 1)]
        public string State { get; set; }

        [MaxLength(9)]
        [ForeignKey("SalesBox")]
        [Column(Order = 0)]
        public string Postcode { get; set; }

        [JsonIgnore]
        public virtual SalesBox SalesBox { get; set; }

        [JsonIgnore]
        public virtual ICollection<Cost> Costs { get; set; }
        [JsonIgnore]
        public virtual ICollection<Quote.Quote> Quotes { get; set; }
    }
}
