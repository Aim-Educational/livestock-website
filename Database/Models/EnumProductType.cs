using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("enum_product_type")]
    public partial class EnumProductType
    {
        public EnumProductType()
        {
            Product = new HashSet<Product>();
        }

        [Column("enum_product_type_id")]
[DisplayName("Enum Product Type")]
        public int EnumProductTypeId { get; set; }
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

        [InverseProperty("EnumProductType")]
        public virtual ICollection<Product> Product { get; set; }
    }
}
