using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("poultry_classification")]
    public partial class PoultryClassification
    {
        [Column("poultry_classification_id")]
[DisplayName("Poultry Classification")]
        public int PoultryClassificationId { get; set; }
        [Column("critter_type_id")]
[DisplayName("Critter Type")]
        public int CritterTypeId { get; set; }
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

        [ForeignKey("CritterTypeId")]
        [InverseProperty("PoultryClassification")]
        public virtual CritterType CritterType { get; set; }
    }
}
