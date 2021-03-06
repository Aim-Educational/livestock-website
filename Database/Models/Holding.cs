using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("holding")]
    public partial class Holding
    {
        public Holding()
        {
            Location = new HashSet<Location>();
        }

        [Column("holding_id")]
[DisplayName("Holding")]
        public int HoldingId { get; set; }
        [Required]
        [Column("holding_number")]
        [StringLength(50)]
[DisplayName("Holding Number")]
        public string HoldingNumber { get; set; }
        [Required]
        [Column("address")]
        [StringLength(100)]
[DisplayName("Address")]
        public string Address { get; set; }
        [Required]
        [Column("postcode")]
        [StringLength(50)]
[DisplayName("Postcode")]
        public string Postcode { get; set; }
        [Required]
        [Column("grid_reference")]
        [StringLength(50)]
[DisplayName("Grid Reference")]
        public string GridReference { get; set; }
        [Column("contact_id")]
[DisplayName("Contact")]
        public int ContactId { get; set; }
        [Column("register_for_pigs")]
[DisplayName("Register For Pigs")]
        public bool RegisterForPigs { get; set; }
        [Column("register_for_sheep_goats")]
[DisplayName("Register For Sheep Goats")]
        public bool RegisterForSheepGoats { get; set; }
        [Column("register_for_cows")]
[DisplayName("Register For Cows")]
        public bool RegisterForCows { get; set; }
        [Column("register_for_fish")]
[DisplayName("Register For Fish")]
        public bool RegisterForFish { get; set; }
        [Column("register_for_poultry")]
[DisplayName("Register For Poultry")]
        public bool RegisterForPoultry { get; set; }
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

        [ForeignKey("ContactId")]
        [InverseProperty("Holding")]
        public virtual Contact Contact { get; set; }
        [InverseProperty("Holding")]
        public virtual ICollection<Location> Location { get; set; }
    }
}
