using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class Breed
    {
[DisplayName("Breed")]
        public int BreedId { get; set; }
[DisplayName("Critter Type")]
        public int CritterTypeId { get; set; }
[DisplayName("Description")]
        public string Description { get; set; }
[DisplayName("Registerable")]
        public bool Registerable { get; set; }
[DisplayName("Breed Society")]
        public int BreedSocietyContactId { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }
    }
}
