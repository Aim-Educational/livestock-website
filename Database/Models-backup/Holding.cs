using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class Holding
    {
[DisplayName("Holding")]
        public int HoldingId { get; set; }
[DisplayName("Holding Number")]
        public string HoldingNumber { get; set; }
[DisplayName("Address")]
        public string Address { get; set; }
[DisplayName("Postcode")]
        public string Postcode { get; set; }
[DisplayName("Grid Reference")]
        public string GridReference { get; set; }
[DisplayName("Contact")]
        public int ContactId { get; set; }
[DisplayName("Register For Pigs")]
        public bool RegisterForPigs { get; set; }
[DisplayName("Register For Sheep Goats")]
        public bool RegisterForSheepGoats { get; set; }
[DisplayName("Register For Cows")]
        public bool RegisterForCows { get; set; }
[DisplayName("Register For Fish")]
        public bool RegisterForFish { get; set; }
[DisplayName("Register For Poultry")]
        public bool RegisterForPoultry { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }
    }
}
