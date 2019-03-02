using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class Role
    {
        public Role()
        {
            MenuHeader = new HashSet<MenuHeader>();
            UserRoleMap = new HashSet<UserRoleMap>();
        }

[DisplayName("Role")]
        public int RoleId { get; set; }
[DisplayName("Description")]
        public string Description { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

        public virtual ICollection<MenuHeader> MenuHeader { get; set; }
        public virtual ICollection<UserRoleMap> UserRoleMap { get; set; }
    }
}
