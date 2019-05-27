using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;

namespace Website.Models
{
    public class GroupCreateEditViewModel
    {
        /// <summary>
        /// This is mostly for if the creation fails, so the user doesn't have to renter data while they fix things.
        /// </summary>
        public AdmuGroup Group { get; set; }

        /// <summary>
        /// Make sure to put the GroupDescriptions into the ViewData collection.
        /// </summary>
        [Required(ErrorMessage = "There must be at least one entity for this group.")]
        [MinLength(1, ErrorMessage = "There must be at least one entity for this group.")]
        public IEnumerable<int> SelectedMemberIds { get; set; }

        public string GroupType { get; set; }
        
        [RegularExpression("create|edit")]
        public string CreateOrEdit { get; set; }
    }
}
