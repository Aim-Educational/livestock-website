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
        public int UserId;
        public AlUserInfo Info;
        public Role Role;
    }
}
