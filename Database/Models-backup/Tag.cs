using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class Tag
    {
[DisplayName("Tag")]
        public int TagId { get; set; }
[DisplayName("Critter")]
        public int CritterId { get; set; }
[DisplayName("Tag1")]
        public string Tag1 { get; set; }
[DisplayName("Rfid")]
        public string Rfid { get; set; }
[DisplayName("Date Time")]
        public DateTime DateTime { get; set; }
[DisplayName("User")]
        public int UserId { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

[DisplayName("Critter")]
        public Critter Critter { get; set; }
    }
}
