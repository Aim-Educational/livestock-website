using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class MenuHeaderItemMap
    {
[DisplayName("Menu Header Item Map")]
        public int MenuHeaderItemMapId { get; set; }
[DisplayName("Menu Header")]
        public int MenuHeaderId { get; set; }
[DisplayName("Menu Item")]
        public int MenuItemId { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

        public virtual MenuHeader MenuHeader { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }
}
