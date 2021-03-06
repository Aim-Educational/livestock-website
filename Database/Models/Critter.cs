using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("critter")]
    public partial class Critter
    {
        public Critter()
        {
            CritterLifeEvent = new HashSet<CritterLifeEvent>();
            InverseDadCritter = new HashSet<Critter>();
            InverseMumCritter = new HashSet<Critter>();
            Tag = new HashSet<Tag>();
        }

        [Column("critter_id")]
[DisplayName("Critter")]
        public int CritterId { get; set; }
        [Column("critter_type_id")]
[DisplayName("Critter Type")]
        public int CritterTypeId { get; set; }
        [Required]
        [Column("gender")]
        [StringLength(1)]
[DisplayName("Gender")]
        public string Gender { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
[DisplayName("Name")]
        public string Name { get; set; }
        [Column("mum_critter_id")]
[DisplayName("Mum Critter")]
        public int MumCritterId { get; set; }
        [Column("dad_critter_id")]
[DisplayName("Dad Critter")]
        public int DadCritterId { get; set; }
        [Column("mum_further")]
        [StringLength(255)]
[DisplayName("Mum Further")]
        public string MumFurther { get; set; }
        [Column("dad_further")]
        [StringLength(255)]
[DisplayName("Dad Further")]
        public string DadFurther { get; set; }
        [Column("owner_contact_id")]
[DisplayName("Owner")]
        public int OwnerContactId { get; set; }
        [Column("breed_id")]
[DisplayName("Breed")]
        public int BreedId { get; set; }
        [Column("critter_image_id")]
[DisplayName("Critter Image")]
        public int? CritterImageId { get; set; }
        [Column("comment")]
        [StringLength(50)]
[DisplayName("Comment")]
        public string Comment { get; set; }
        [Required]
        [Column("timestamp")]
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
        [Column("version_number")]
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }
        [Column("flags")]
[DisplayName("Flags")]
        public int Flags { get; set; }
        [Required]
        [Column("tag_number")]
        [StringLength(50)]
[DisplayName("Tag Number")]
        public string TagNumber { get; set; }

        [ForeignKey("BreedId")]
        [InverseProperty("Critter")]
        public virtual Breed Breed { get; set; }
        [ForeignKey("CritterImageId")]
        [InverseProperty("Critter")]
        public virtual CritterImage CritterImage { get; set; }
        [ForeignKey("CritterTypeId")]
        [InverseProperty("Critter")]
        public virtual CritterType CritterType { get; set; }
        [ForeignKey("DadCritterId")]
        [InverseProperty("InverseDadCritter")]
        public virtual Critter DadCritter { get; set; }
        [ForeignKey("MumCritterId")]
        [InverseProperty("InverseMumCritter")]
        public virtual Critter MumCritter { get; set; }
        [ForeignKey("OwnerContactId")]
        [InverseProperty("Critter")]
        public virtual Contact OwnerContact { get; set; }
        [InverseProperty("Critter")]
        public virtual ICollection<CritterLifeEvent> CritterLifeEvent { get; set; }
        [InverseProperty("DadCritter")]
        public virtual ICollection<Critter> InverseDadCritter { get; set; }
        [InverseProperty("MumCritter")]
        public virtual ICollection<Critter> InverseMumCritter { get; set; }
        [InverseProperty("Critter")]
        public virtual ICollection<Tag> Tag { get; set; }
    }
}
