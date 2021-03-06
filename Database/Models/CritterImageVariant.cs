using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("critter_image_variant")]
    public partial class CritterImageVariant
    {
        [Column("critter_image_variant_id")]
[DisplayName("Critter Image Variant")]
        public int CritterImageVariantId { get; set; }
        [Column("critter_image_original_id")]
[DisplayName("Critter Image Original")]
        public int CritterImageOriginalId { get; set; }
        [Column("critter_image_modified_id")]
[DisplayName("Critter Image Modified")]
        public int CritterImageModifiedId { get; set; }
        [Column("width")]
[DisplayName("Width")]
        public int Width { get; set; }
        [Column("height")]
[DisplayName("Height")]
        public int Height { get; set; }
        [Required]
        [Column("timestamp")]
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }

        [ForeignKey("CritterImageModifiedId")]
        [InverseProperty("CritterImageVariantCritterImageModified")]
        public virtual CritterImage CritterImageModified { get; set; }
        [ForeignKey("CritterImageOriginalId")]
        [InverseProperty("CritterImageVariantCritterImageOriginal")]
        public virtual CritterImage CritterImageOriginal { get; set; }
    }
}
