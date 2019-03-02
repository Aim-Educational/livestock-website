using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class Critter
    {
        public Critter()
        {
            CritterLifeEvent = new HashSet<CritterLifeEvent>();
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

        public virtual Breed Breed { get; set; }
        public virtual CritterType CritterType { get; set; }
        public virtual Critter DadCritter { get; set; }
        public virtual Critter MumCritter { get; set; }
        public virtual ICollection<CritterLifeEvent> CritterLifeEvent { get; set; }
        public virtual ICollection<Critter> InverseDadCritter { get; set; }
        public virtual ICollection<Critter> InverseMumCritter { get; set; }
        public virtual ICollection<Tag> Tag { get; set; }
    }
}
