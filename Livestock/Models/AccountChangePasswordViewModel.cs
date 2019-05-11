using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models
{
    public class AccountChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [DisplayName("Current Password")]
        [Required]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm New Password")]
        [Required]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }
    }
}
