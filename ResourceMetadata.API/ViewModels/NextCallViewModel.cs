using System.ComponentModel.DataAnnotations;
using DataAccess.EntityFramework.TypeLibrary;

namespace ResourceMetadata.API.ViewModels
{
    /// <summary>
    /// view model for generate next available contact
    /// </summary>
    public class NextCallViewModel
    {
        /// <summary>
        /// initial of the person requesting the call
        /// </summary>
        public string Initial { get; set; }

        /// <summary>
        /// if a site Id if provided, the call will generated from the site instead of from a call queue
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Incorrect site id")]
        public int? SiteId { get; set; }

        /// <summary>
        /// existing call id that has not been ended
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Incorrect last call id")]
        public int? LastCallId { get; set; }
    }
}