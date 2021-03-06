using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("breed")]
    public partial class Breed
    {
        public Breed()
        {
            Critter = new HashSet<Critter>();
        }

        [Column("breed_id")]
[DisplayName("Breed")]
        public int BreedId { get; set; }
        [Column("critter_type_id")]
[DisplayName("Critter Type")]
        public int CritterTypeId { get; set; }
        [Required]
        [Column("description")]
        [StringLength(50)]
[DisplayName("Description")]
        public string Description { get; set; }
        [Column("registerable")]
[DisplayName("Registerable")]
        public bool Registerable { get; set; }
        [Column("breed_society_contact_id")]
[DisplayName("Breed Society")]
        public int BreedSocietyContactId { get; set; }
        [Required]
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

        [ForeignKey("BreedSocietyContactId")]
        [InverseProperty("Breed")]
        public virtual Contact BreedSocietyContact { get; set; }
        [ForeignKey("CritterTypeId")]
        [InverseProperty("Breed")]
        public virtual CritterType CritterType { get; set; }
        [InverseProperty("Breed")]
        public virtual ICollection<Critter> Critter { get; set; }
    }
}
