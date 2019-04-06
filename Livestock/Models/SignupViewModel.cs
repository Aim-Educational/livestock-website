using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Website.Other;

namespace Website.Models
{
    public class SignupViewModel
    {
        [Required]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Required]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }

        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        [Required]
        public string EmailAddress { get; set; }

        [MustBeTrue(ErrorMessage = "You must agree to the Terms of Service agreement.")]
        [Required]
        public bool ToSConsent { get; set; }
        
        [MustBeTrue(ErrorMessage = "You must accept the terms of the Privacy Statement.")]
        [Required]
        public bool PrivacyConsent { get; set; }
    }
}
