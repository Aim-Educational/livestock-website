using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class MenuItem
    {
        public MenuItem()
        {
            MenuHeaderItemMap = new HashSet<MenuHeaderItemMap>();
        }

[DisplayName("Menu Item")]
        public int MenuItemId { get; set; }
[DisplayName("Title")]
        public string Title { get; set; }
[DisplayName("Icon Uri")]
        public string IconUri { get; set; }
[DisplayName("Role")]
        public int RoleId { get; set; }
[DisplayName("Sequence Number")]
        public int SequenceNumber { get; set; }
[DisplayName("Controller")]
        public string Controller { get; set; }
[DisplayName("Action")]
        public string Action { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

        public virtual ICollection<MenuHeaderItemMap> MenuHeaderItemMap { get; set; }
    }
}
