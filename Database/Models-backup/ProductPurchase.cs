using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class ProductPurchase
    {
[DisplayName("Product Purchase")]
        public int ProductPurchaseId { get; set; }
[DisplayName("Date Time")]
        public DateTime DateTime { get; set; }
[DisplayName("Product")]
        public int ProductId { get; set; }
[DisplayName("Supplier")]
        public int SupplierId { get; set; }
[DisplayName("Location")]
        public int LocationId { get; set; }
[DisplayName("Expiry")]
        public DateTime Expiry { get; set; }
[DisplayName("Cost")]
        public decimal Cost { get; set; }
[DisplayName("Batch Number")]
        public string BatchNumber { get; set; }
[DisplayName("Volume")]
        public double Volume { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

[DisplayName("Product")]
        public Product Product { get; set; }
[DisplayName("Supplier")]
        public Contact Supplier { get; set; }
    }
}
