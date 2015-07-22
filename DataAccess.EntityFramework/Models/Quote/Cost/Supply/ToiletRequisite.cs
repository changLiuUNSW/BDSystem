using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote.Cost.Supply
{
    [Table("ToiletRequisite", Schema = "Quote")]
    public class ToiletRequisite
    {
        [Key]
        public string ItemCode { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool RarelyUsed { get; set; }
        public string Size { get; set; }
        public string Quality { get; set; }
        public string Ply { get; set; }
        public string UnitsPerCarton { get; set; }
        public decimal Cost { get; set; }

        public string HayesCode { get; set; }

       
        public decimal Price { get; set; }

        [NotMapped]
        public decimal Gst
        {
            get { return
                Math.Round(Price * (decimal)0.1, 4);
                }
        }

        [NotMapped]
        public decimal PriceIncludeGst
        {
            get { return Math.Round(Price + Gst, 4);}
        }


    }
}
