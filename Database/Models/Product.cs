using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("product")]
    public partial class Product
    {
        public Product()
        {
            ProductPurchase = new HashSet<ProductPurchase>();
        }

        [Column("product_id")]
[DisplayName("Product")]
        public int ProductId { get; set; }
        [Column("enum_product_type_id")]
[DisplayName("Enum Product Type")]
        public int EnumProductTypeId { get; set; }
        [Required]
        [Column("description")]
        [StringLength(100)]
[DisplayName("Description")]
        public string Description { get; set; }
        [Column("default_volume")]
[DisplayName("Default Volume")]
        public double DefaultVolume { get; set; }
        [Column("requires_refridgeration")]
[DisplayName("Requires Refridgeration")]
        public bool RequiresRefridgeration { get; set; }
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

        [ForeignKey("EnumProductTypeId")]
        [InverseProperty("Product")]
        public virtual EnumProductType EnumProductType { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductPurchase> ProductPurchase { get; set; }
    }
}
