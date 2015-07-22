using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.EntityFramework.Models.BD;
using DataAccess.EntityFramework.Models.BD.Allocation;
using DataAccess.EntityFramework.Models.BD.Site;
using Newtonsoft.Json;

namespace DataAccess.EntityFramework.Models.Quote
{
    public abstract class QuoteBase
    {
        [ForeignKey("Site")]
        public int? SiteId { get; set; }
        [ForeignKey("TempSite")]
        public int? TempSiteId { get; set; }

        [JsonIgnore]
        public Site Site { get; set; }
        [JsonIgnore]
        public TempSite TempSite { get; set; }

        public string Company { get; set; }

        public Address Address { get; set; }

        [MaxLength(25)]
        [ForeignKey("SalesBox")]
        [Column(Order = 1)]
        [Required]
        public string State { get; set; }

        [MaxLength(9)]
        [ForeignKey("SalesBox")]
        [Column(Order = 0)]
        [Required]
        public string Postcode { get; set; }

        public virtual SalesBox SalesBox { get; set; }
    }
}
