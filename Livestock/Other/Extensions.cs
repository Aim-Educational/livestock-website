using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AimLogin.DbModel;
using AimLogin.Services;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Website.Other
{
    public enum RoleEnum
    {
        Student,
        Staff,
        Admin
    }

    public static class LivestockMiscExtensions
    {
        public static async Task SetRoleFor(this IAimUserDataMapManager data, User user, RoleEnum role, LivestockContext db)
        {
            Contract.Requires(user != null);
            Contract.Requires(data != null);
            Contract.Requires(db != null);
            
            var roleName = (role == RoleEnum.Admin) 
                           ? "admin"
                           : (role == RoleEnum.Staff)
                             ? "staff"
                             : "student";

            var dbRole = await db.Role.FirstOrDefaultAsync(r => r.Description == roleName);
            if(dbRole == null)
            {
                dbRole = new Role
                {
                    Comment = "N/A",
                    Description = roleName,
                    VersionNumber = 1
                };
                await db.Role.AddAsync(dbRole);
                await db.SaveChangesAsync();
            }

            await data.SetSingleReferenceFor(user, dbRole);
        }
    }
}
