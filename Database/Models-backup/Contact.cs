using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class Contact
    {
        public Contact()
        {
            ProductPurchase = new HashSet<ProductPurchase>();
        }

[DisplayName("Contact")]
        public int ContactId { get; set; }
[DisplayName("Name")]
        public string Name { get; set; }
[DisplayName("Address")]
        public string Address { get; set; }
[DisplayName("Is Customer")]
        public bool IsCustomer { get; set; }
[DisplayName("Is Supplier")]
        public bool IsSupplier { get; set; }
[DisplayName("Phone Number1")]
        public string PhoneNumber1 { get; set; }
[DisplayName("Phone Number2")]
        public string PhoneNumber2 { get; set; }
[DisplayName("Phone Number3")]
        public string PhoneNumber3 { get; set; }
[DisplayName("Phone Number4")]
        public string PhoneNumber4 { get; set; }
[DisplayName("Email Address")]
        public string EmailAddress { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

        public ICollection<ProductPurchase> ProductPurchase { get; set; }
    }
}
