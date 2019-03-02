using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class CritterType
    {
        public CritterType()
        {
            Breed = new HashSet<Breed>();
            Critter = new HashSet<Critter>();
            PoultryClassification = new HashSet<PoultryClassification>();
        }

[DisplayName("Critter Type")]
        public int CritterTypeId { get; set; }
[DisplayName("Name")]
        public string Name { get; set; }
[DisplayName("Gestration Period")]
        public int GestrationPeriod { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

        public virtual ICollection<Breed> Breed { get; set; }
        public virtual ICollection<Critter> Critter { get; set; }
        public virtual ICollection<PoultryClassification> PoultryClassification { get; set; }
    }
}
