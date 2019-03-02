using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class UserRoleMap
    {
[DisplayName("User Role Map")]
        public int UserRoleMapId { get; set; }
[DisplayName("User")]
        public int UserId { get; set; }
[DisplayName("Role")]
        public int RoleId { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
