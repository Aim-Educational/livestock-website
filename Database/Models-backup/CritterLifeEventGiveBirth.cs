using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class CritterLifeEventGiveBirth
    {
[DisplayName("Critter Life Event Give Birth")]
        public int CritterLifeEventGiveBirthId { get; set; }
[DisplayName("Date Time")]
        public DateTime DateTime { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }
    }
}
