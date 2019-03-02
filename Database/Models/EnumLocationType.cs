using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class EnumLocationType
    {
        public EnumLocationType()
        {
            Location = new HashSet<Location>();
        }

[DisplayName("Enum Location Type")]
        public int EnumLocationTypeId { get; set; }
[DisplayName("Description")]
        public string Description { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

        public virtual ICollection<Location> Location { get; set; }
    }
}
