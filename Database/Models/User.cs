using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class User
    {
        public User()
        {
            UserMobileNumber = new HashSet<UserMobileNumber>();
            UserRoleMap = new HashSet<UserRoleMap>();
        }

[DisplayName("User")]
        public int UserId { get; set; }
[DisplayName("Name")]
        public string Name { get; set; }
[DisplayName("Email")]
        public string Email { get; set; }
[DisplayName("Preferred Mobile Number")]
        public int PreferredMobileNumberId { get; set; }
[DisplayName("Nickname")]
        public string Nickname { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

        public virtual UserMobileNumber PreferredMobileNumber { get; set; }
        public virtual ICollection<UserMobileNumber> UserMobileNumber { get; set; }
        public virtual ICollection<UserRoleMap> UserRoleMap { get; set; }
    }
}
