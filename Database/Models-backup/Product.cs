using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductPurchase = new HashSet<ProductPurchase>();
        }

[DisplayName("Product")]
        public int ProductId { get; set; }
[DisplayName("Enum Product Type")]
        public int EnumProductTypeId { get; set; }
[DisplayName("Description")]
        public string Description { get; set; }
[DisplayName("Default Volume")]
        public double DefaultVolume { get; set; }
[DisplayName("Requires Refridgeration")]
        public bool RequiresRefridgeration { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

[DisplayName("Enum Product Type")]
        public EnumProductType EnumProductType { get; set; }
        public ICollection<ProductPurchase> ProductPurchase { get; set; }
    }
}
