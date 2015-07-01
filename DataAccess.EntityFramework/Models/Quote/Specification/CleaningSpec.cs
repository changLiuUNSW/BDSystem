using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote.Specification
{
    [Table("Spec", Schema = "Quote")]
    public class CleaningSpec
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<SpecItem> Items { get; set; } 
        public virtual ICollection<SpecTitle> Titles { get; set; } 
    }

    [Table("SpecItem", Schema = "Quote")]
    public class SpecItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Spec")]
        public int SpecId { get; set; }
        public virtual CleaningSpec Spec { get; set; }

        [ForeignKey("Title")]
        public int TitleId { get; set; }
        public virtual SpecTitle Title { get; set; }

        public string Description { get; set; }
        public string Frequency { get; set; }
        public int? TimesPerFrequency { get; set; }
    }

    [Table("SpecTitle", Schema = "Quote")]
    public class SpecTitle
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }

        [ForeignKey("Spec")]
        public int SpecId { get; set; }
        public virtual CleaningSpec Spec { get; set; }

        public virtual ICollection<SpecItem> Items { get; set; }
    }
}
