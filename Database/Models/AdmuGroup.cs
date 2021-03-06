using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("admu_group")]
    public partial class AdmuGroup
    {
        [Column("admu_group_id")]
[DisplayName("Admu Group")]
        public int AdmuGroupId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(100)]
[DisplayName("Name")]
        public string Name { get; set; }
        [Required]
        [Column("description")]
        [StringLength(255)]
[DisplayName("Description")]
        public string Description { get; set; }
        [Column("group_type")]
[DisplayName("Group Type")]
        public int GroupType { get; set; }
        [Required]
        [Column("timestamp")]
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
    }
}
