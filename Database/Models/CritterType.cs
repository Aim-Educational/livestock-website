using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("critter_type")]
    public partial class CritterType
    {
        public CritterType()
        {
            Breed = new HashSet<Breed>();
            Critter = new HashSet<Critter>();
            PoultryClassification = new HashSet<PoultryClassification>();
        }

        [Column("critter_type_id")]
[DisplayName("Critter Type")]
        public int CritterTypeId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
[DisplayName("Name")]
        public string Name { get; set; }
        [Column("gestration_period")]
[DisplayName("Gestration Period")]
        public int GestrationPeriod { get; set; }
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

        [InverseProperty("CritterType")]
        public virtual ICollection<Breed> Breed { get; set; }
        [InverseProperty("CritterType")]
        public virtual ICollection<Critter> Critter { get; set; }
        [InverseProperty("CritterType")]
        public virtual ICollection<PoultryClassification> PoultryClassification { get; set; }
    }
}
