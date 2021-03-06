using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("location")]
    public partial class Location
    {
        public Location()
        {
            InverseParent = new HashSet<Location>();
            ProductPurchase = new HashSet<ProductPurchase>();
        }

        [Column("location_id")]
[DisplayName("Location")]
        public int LocationId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
[DisplayName("Name")]
        public string Name { get; set; }
        [Column("enum_location_type_id")]
[DisplayName("Enum Location Type")]
        public int EnumLocationTypeId { get; set; }
        [Column("parent_id")]
[DisplayName("Parent")]
        public int ParentId { get; set; }
        [Column("holding_id")]
[DisplayName("Holding")]
        public int HoldingId { get; set; }
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

        [ForeignKey("EnumLocationTypeId")]
        [InverseProperty("Location")]
        public virtual EnumLocationType EnumLocationType { get; set; }
        [ForeignKey("HoldingId")]
        [InverseProperty("Location")]
        public virtual Holding Holding { get; set; }
        [ForeignKey("ParentId")]
        [InverseProperty("InverseParent")]
        public virtual Location Parent { get; set; }
        [InverseProperty("Parent")]
        public virtual ICollection<Location> InverseParent { get; set; }
        [InverseProperty("Location")]
        public virtual ICollection<ProductPurchase> ProductPurchase { get; set; }
    }
}
