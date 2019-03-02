using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class UserMobileNumber
    {
        public UserMobileNumber()
        {
            User = new HashSet<User>();
        }

[DisplayName("User Mobile Number")]
        public int UserMobileNumberId { get; set; }
[DisplayName("User")]
        public int UserId { get; set; }
[DisplayName("Mobile Number")]
        public string MobileNumber { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

        public virtual User UserNavigation { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
