using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("menu_item")]
    public partial class MenuItem
    {
        public MenuItem()
        {
            MenuHeaderItemMap = new HashSet<MenuHeaderItemMap>();
        }

        [Column("menu_item_id")]
[DisplayName("Menu Item")]
        public int MenuItemId { get; set; }
        [Required]
        [Column("title")]
        [StringLength(50)]
[DisplayName("Title")]
        public string Title { get; set; }
        [Required]
        [Column("icon_uri")]
        [StringLength(255)]
[DisplayName("Icon Uri")]
        public string IconUri { get; set; }
        [Column("role_id")]
[DisplayName("Role")]
        public int RoleId { get; set; }
        [Column("sequence_number")]
[DisplayName("Sequence Number")]
        public int SequenceNumber { get; set; }
        [Required]
        [Column("controller")]
        [StringLength(50)]
[DisplayName("Controller")]
        public string Controller { get; set; }
        [Required]
        [Column("action")]
        [StringLength(50)]
[DisplayName("Action")]
        public string Action { get; set; }
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

        [InverseProperty("MenuItem")]
        public virtual ICollection<MenuHeaderItemMap> MenuHeaderItemMap { get; set; }
    }
}
