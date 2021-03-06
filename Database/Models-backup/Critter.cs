using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class Critter
    {
        public Critter()
        {
            InverseDadCritter = new HashSet<Critter>();
            InverseMumCritter = new HashSet<Critter>();
            Tag = new HashSet<Tag>();
        }

[DisplayName("Critter")]
        public int CritterId { get; set; }
[DisplayName("Critter Type")]
        public int CritterTypeId { get; set; }
[DisplayName("Gender")]
        public string Gender { get; set; }
[DisplayName("Name")]
        public string Name { get; set; }
[DisplayName("Mum Critter")]
        public int MumCritterId { get; set; }
[DisplayName("Dad Critter")]
        public int DadCritterId { get; set; }
[DisplayName("Mum Further")]
        public string MumFurther { get; set; }
[DisplayName("Dad Further")]
        public string DadFurther { get; set; }
[DisplayName("Owner")]
        public int OwnerContactId { get; set; }
[DisplayName("Breed")]
        public int BreedId { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

[DisplayName("Critter Type")]
        public CritterType CritterType { get; set; }
[DisplayName("Dad Critter")]
        public Critter DadCritter { get; set; }
[DisplayName("Mum Critter")]
        public Critter MumCritter { get; set; }
        public ICollection<Critter> InverseDadCritter { get; set; }
        public ICollection<Critter> InverseMumCritter { get; set; }
        public ICollection<Tag> Tag { get; set; }
    }
}
