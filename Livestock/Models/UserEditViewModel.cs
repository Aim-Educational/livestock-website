using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AimLogin.DbModel;
using Database.Models;


namespace Website.Models
{
    public class UserEditViewModel
    {
        public int UserId { get; set; }
        public AlUserInfo Info { get; set; }
        public Role Role { get; set; }
        public UserEmail Email { get; set; }
    }
}
