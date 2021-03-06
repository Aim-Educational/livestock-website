using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("critter_image")]
    public partial class CritterImage
    {
        public CritterImage()
        {
            Critter = new HashSet<Critter>();
            CritterImageVariantCritterImageModified = new HashSet<CritterImageVariant>();
            CritterImageVariantCritterImageOriginal = new HashSet<CritterImageVariant>();
        }

        [Column("critter_image_id")]
[DisplayName("Critter Image")]
        public int CritterImageId { get; set; }
        [Required]
        [Column("data")]
[DisplayName("Data")]
        public byte[] Data { get; set; }
        [Required]
        [Column("timestamp")]
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }

        [InverseProperty("CritterImage")]
        public virtual ICollection<Critter> Critter { get; set; }
        [InverseProperty("CritterImageModified")]
        public virtual ICollection<CritterImageVariant> CritterImageVariantCritterImageModified { get; set; }
        [InverseProperty("CritterImageOriginal")]
        public virtual ICollection<CritterImageVariant> CritterImageVariantCritterImageOriginal { get; set; }
    }
}
