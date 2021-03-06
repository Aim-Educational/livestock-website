using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class Location
    {
[DisplayName("Location")]
        public int LocationId { get; set; }
[DisplayName("Name")]
        public string Name { get; set; }
[DisplayName("Enum Location Type")]
        public int EnumLocationTypeId { get; set; }
[DisplayName("Parent")]
        public int ParentId { get; set; }
[DisplayName("Holding")]
        public int HoldingId { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }
    }
}
