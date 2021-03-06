using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("role")]
    public partial class Role
    {
        public Role()
        {
            MenuHeader = new HashSet<MenuHeader>();
        }

        [Column("role_id")]
[DisplayName("Role")]
        public int RoleId { get; set; }
        [Required]
        [Column("description")]
        [StringLength(50)]
[DisplayName("Description")]
        public string Description { get; set; }
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

        [InverseProperty("Role")]
        public virtual ICollection<MenuHeader> MenuHeader { get; set; }
    }
}
