using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
