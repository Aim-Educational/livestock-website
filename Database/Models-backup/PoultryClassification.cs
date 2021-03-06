using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class PoultryClassification
    {
[DisplayName("Poultry Classification")]
        public int PoultryClassificationId { get; set; }
[DisplayName("Critter Type")]
        public int CritterTypeId { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

[DisplayName("Critter Type")]
        public CritterType CritterType { get; set; }
    }
}
