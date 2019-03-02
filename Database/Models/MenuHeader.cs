using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class MenuHeader
    {
        public MenuHeader()
        {
            InverseMenuHeaderParent = new HashSet<MenuHeader>();
            MenuItem = new HashSet<MenuItem>();
        }

[DisplayName("Menu Header")]
        public int MenuHeaderId { get; set; }
[DisplayName("Name")]
        public string Name { get; set; }
[DisplayName("Title")]
        public string Title { get; set; }
[DisplayName("Application Code")]
        public int ApplicationCode { get; set; }
[DisplayName("Role")]
        public int RoleId { get; set; }
[DisplayName("Menu Header Parent")]
        public int MenuHeaderParentId { get; set; }
[DisplayName("Image Uri")]
        public string ImageUri { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

        public virtual MenuHeader MenuHeaderParent { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<MenuHeader> InverseMenuHeaderParent { get; set; }
        public virtual ICollection<MenuItem> MenuItem { get; set; }
    }
}
