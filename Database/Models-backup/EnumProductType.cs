using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class EnumProductType
    {
        public EnumProductType()
        {
            Product = new HashSet<Product>();
        }

[DisplayName("Enum Product Type")]
        public int EnumProductTypeId { get; set; }
[DisplayName("Description")]
        public string Description { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

        public ICollection<Product> Product { get; set; }
    }
}
