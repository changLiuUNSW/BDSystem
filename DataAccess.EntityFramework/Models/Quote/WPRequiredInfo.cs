using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote
{
    [Table("WPRequiredInfo", Schema = "Quote")]
    public class WpRequiredInfo
    {
        [Key, ForeignKey("Quote")]
        public int Id { get; set; }

        [Description("Date or time quote required")]
        public DateTime? RequiredDate { get; set; }

        [Description("Method of delivery")]
        public string DeliveryMethod { get; set; }

        [Description("Client's website address")]
        public string WebAddress { get; set; }

        [Description("References - nominate 3")]
        public string Reference { get; set; }

        [Description("LTT assisted with costing or questions")]
        public string AssistedLtt { get; set; }

        [Description("Other OP who assisted and walk around")]
        public string AssistedQp { get; set; }

        [Description("Have you checked the price with the client")]
        public string PriceCheck { get; set; }

        [Description("Did you change the price after checking price with client")]
        public string PriceChange { get; set; }

        [Description("Did you discuss with client")]
        public bool? DiscussWithClient { get; set; }

        [Description("When will the work be done?")]
        public string EstimateFinish { get; set; }

        [Description("Description of Area/Surface")]
        public string AreaDescription { get; set; }

        [Description("Description of work to be undertaken")]
        public string WorkDescription { get; set; }

        [Description("Additional information")]
        public string AdditionalInformation { get; set; }

        public virtual Quote Quote { get; set; }
    }
}