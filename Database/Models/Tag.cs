using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("tag")]
    public partial class Tag
    {
        [Column("tag_id")]
[DisplayName("Tag")]
        public int TagId { get; set; }
        [Column("critter_id")]
[DisplayName("Critter")]
        public int CritterId { get; set; }
        [Required]
        [Column("tag")]
        [StringLength(50)]
[DisplayName("Tag1")]
        public string Tag1 { get; set; }
        [Required]
        [Column("rfid")]
        [StringLength(255)]
[DisplayName("Rfid")]
        public string Rfid { get; set; }
        [Column("date_time", TypeName = "datetime")]
[DisplayName("Date Time")]
        public DateTime DateTime { get; set; }
        [Column("user_id")]
[DisplayName("User")]
        public int UserId { get; set; }
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

        [ForeignKey("CritterId")]
        [InverseProperty("Tag")]
        public virtual Critter Critter { get; set; }
    }
}
