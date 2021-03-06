using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("enum_location_type")]
    public partial class EnumLocationType
    {
        public EnumLocationType()
        {
            Location = new HashSet<Location>();
        }

        [Column("enum_location_type_id")]
[DisplayName("Enum Location Type")]
        public int EnumLocationTypeId { get; set; }
        [Required]
        [Column("description")]
        [StringLength(50)]
[DisplayName("Description")]
        public string Description { get; set; }
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

        [InverseProperty("EnumLocationType")]
        public virtual ICollection<Location> Location { get; set; }
    }
}
