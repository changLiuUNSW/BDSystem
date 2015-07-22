using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace ResourceMetadata.API.ViewModels
{
    /// <summary>
    /// view model for call sheet end process
    /// </summary>
    public class EndCallViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int SiteId { get; set; }

        /// <summary>
        /// initial of the caller
        /// </summary>
        public string Initial { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? ContactId { get; set; }

        /// <summary>
        /// id for person who gets assigned for lead
        /// </summary>
        public int? LeadPersonId { get; set; }

        /// <summary>
        /// id for removing occupid contact status
        /// </summary>
        public int? OccupiedId { get; set; }

        /// <summary>
        /// redirect url for lead
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// actions needs to be performed during the end process
        /// </summary>
        public JObject Actions { get; set; }
    }
}