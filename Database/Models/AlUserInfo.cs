using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("al_user_info")]
    public partial class AlUserInfo
    {
        [Column("al_user_info_id")]
[DisplayName("Al User Info")]
        public int AlUserInfoId { get; set; }
        [Required]
        [Column("first_name")]
        [StringLength(150)]
[DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [Column("last_name")]
        [StringLength(150)]
[DisplayName("Last Name")]
        public string LastName { get; set; }
        [Column("tos_consent")]
[DisplayName("Tos Consent")]
        public bool TosConsent { get; set; }
        [Column("privacy_consent")]
[DisplayName("Privacy Consent")]
        public bool PrivacyConsent { get; set; }
        [Required]
        [Column("timestamp")]
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
        [Required]
        [Column("comment")]
        [StringLength(50)]
[DisplayName("Comment")]
        public string Comment { get; set; }
    }
}
