using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("product_purchase")]
    public partial class ProductPurchase
    {
        [Column("product_purchase_id")]
[DisplayName("Product Purchase")]
        public int ProductPurchaseId { get; set; }
        [Column("date_time", TypeName = "datetime")]
[DisplayName("Date Time")]
        public DateTime DateTime { get; set; }
        [Column("product_id")]
[DisplayName("Product")]
        public int ProductId { get; set; }
        [Column("supplier_id")]
[DisplayName("Supplier")]
        public int SupplierId { get; set; }
        [Column("location_id")]
[DisplayName("Location")]
        public int LocationId { get; set; }
        [Column("expiry", TypeName = "date")]
[DisplayName("Expiry")]
        public DateTime Expiry { get; set; }
        [Column("cost", TypeName = "money")]
[DisplayName("Cost")]
        public decimal Cost { get; set; }
        [Required]
        [Column("batch_number")]
        [StringLength(50)]
[DisplayName("Batch Number")]
        public string BatchNumber { get; set; }
        [Column("volume")]
[DisplayName("Volume")]
        public double Volume { get; set; }
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

        [ForeignKey("LocationId")]
        [InverseProperty("ProductPurchase")]
        public virtual Location Location { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("ProductPurchase")]
        public virtual Product Product { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("ProductPurchase")]
        public virtual Contact Supplier { get; set; }
    }
}
