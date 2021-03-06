using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("contact")]
    public partial class Contact
    {
        public Contact()
        {
            Breed = new HashSet<Breed>();
            Critter = new HashSet<Critter>();
            Holding = new HashSet<Holding>();
            ProductPurchase = new HashSet<ProductPurchase>();
        }

        [Column("contact_id")]
[DisplayName("Contact")]
        public int ContactId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
[DisplayName("Name")]
        public string Name { get; set; }
        [Required]
        [Column("address")]
        [StringLength(100)]
[DisplayName("Address")]
        public string Address { get; set; }
        [Column("is_customer")]
[DisplayName("Is Customer")]
        public bool IsCustomer { get; set; }
        [Column("is_supplier")]
[DisplayName("Is Supplier")]
        public bool IsSupplier { get; set; }
        [Required]
        [Column("phone_number1")]
        [StringLength(50)]
[DisplayName("Phone Number1")]
        public string PhoneNumber1 { get; set; }
        [Required]
        [Column("phone_number2")]
        [StringLength(50)]
[DisplayName("Phone Number2")]
        public string PhoneNumber2 { get; set; }
        [Required]
        [Column("phone_number3")]
        [StringLength(50)]
[DisplayName("Phone Number3")]
        public string PhoneNumber3 { get; set; }
        [Required]
        [Column("phone_number4")]
        [StringLength(50)]
[DisplayName("Phone Number4")]
        public string PhoneNumber4 { get; set; }
        [Required]
        [Column("email_address")]
        [StringLength(100)]
[DisplayName("Email Address")]
        public string EmailAddress { get; set; }
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

        [InverseProperty("BreedSocietyContact")]
        public virtual ICollection<Breed> Breed { get; set; }
        [InverseProperty("OwnerContact")]
        public virtual ICollection<Critter> Critter { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<Holding> Holding { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<ProductPurchase> ProductPurchase { get; set; }
    }
}
