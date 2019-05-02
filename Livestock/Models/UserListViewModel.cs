using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AimLogin.DbModel;
using Database.Models;

namespace Website.Models
{
    public class UserListViewModel
    {
        public User user;
        public Role role;
        public AlUserInfo info;
        public UserEmail email;
    }
}
