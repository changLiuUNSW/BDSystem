using System.ComponentModel.DataAnnotations;

namespace ResourceMetadata.API.ViewModels
{
    /// <summary>
    /// View model for crete a new lead
    /// </summary>
    public class GenerateLeadViewModel
    {
        /// <summary>
        /// contact id 
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Not a valid contact")]
        public int ContactId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Not a valid lead person")]
        public int? LeadPersonId { get; set; }
    }
}