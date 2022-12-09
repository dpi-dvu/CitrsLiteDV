using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitrsLite.Data.Models
{
    /// <summary>
    /// The data model for the entity that deals with trees, either sending or receiving budwood
    /// </summary>
    public class Participant
    {
        /// <summary>
        /// The Id number of the Participant
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of the Participant
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// The type of Participant (Government/Research/Nursery, etc.)
        /// </summary>
        [Required]
        public string Type { get; set; }
        /// <summary>
        /// A brief summary or collection of comments about the Participant
        /// </summary>
        [StringLength(1000)]
        public string? Description { get; set; }
        /// <summary>
        /// The phone number for the Participant
        /// </summary>
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// The address of the Participant
        /// </summary>
        [Required]
        public string Address { get; set; }
        /// <summary>
        /// The city of where the Participant is located
        /// </summary>
        [Required]
        public string City { get; set; }
        /// <summary>
        /// The state of where the Participant is located
        /// </summary>
        [Required]
        public string State { get; set; } = "FL";
        /// <summary>
        /// Boolean value of whether the Participant is active
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// The user that initialized the Participant in CITRSLite
        /// </summary>
        [Required]
        public string CreatedBy { get; set; }
        /// <summary>
        /// The date of when the Participant was introduced
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// The user that last updated the Participant
        /// </summary>
        [Required]
        public string ModifiedBy { get; set;}
        /// <summary>
        /// The date of when Participant was last updated
        /// </summary>
        public DateTime ModificationDate { get; set;}

    }
}