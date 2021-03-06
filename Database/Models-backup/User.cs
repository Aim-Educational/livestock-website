using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class User
    {
[DisplayName("User")]
        public int UserId { get; set; }
[DisplayName("Name")]
        public string Name { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }
    }
}
