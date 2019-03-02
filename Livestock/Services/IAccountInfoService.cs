using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Website.Services
{
    public enum UserPermission : int
    {
        ReadOnly = 0,
        LivestockModify = 1 << 0
    }

    public interface IAccountInfoService
    {
        Task<User> GetUserByEmailAsync(string email);
        void AddClaimsFor(User user, ClaimsIdentity identity);
        void AddTemporaryUserClaims(ClaimsIdentity identity);
        Task<Role> GetRoleByNameAsync(string name);
        Task<IEnumerable<Role>> GetRolesForUserAsync(User user);
    }

    public class AccountInfoService : IAccountInfoService
    {
        public LivestockContext Db;

        public AccountInfoService(LivestockContext db)
        {
            this.Db = db;
        }

        public async void AddClaimsFor(User user, ClaimsIdentity identity)
        {
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Email));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Name ?? "NO NAME FOUND"));

            var roles = await this.GetRolesForUserAsync(user);
            foreach (var role in roles)
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Description));
        }

        public void AddTemporaryUserClaims(ClaimsIdentity identity)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, "temp"));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "unknown@unknown.com"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "TODO:Names"));
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return this.Db.User
                          .Include(u => u.UserRoleMap)
                          .ThenInclude(map => map.Role)
                          .FirstOrDefaultAsync(user => user.Email == email);
        }

        public Task<Role> GetRoleByNameAsync(string name)
        {
            return this.Db.Role.FirstOrDefaultAsync(r => r.Description == name);
        }

        public Task<IEnumerable<Role>> GetRolesForUserAsync(User user)
        {
            return Task.Run(() => user.UserRoleMap.Select(map => map.Role));
        }
    }

    public static class IAccountInfoServiceExtensions
    {
        public static Task<User> GetUserByPrincipleAsync(this IAccountInfoService account, ClaimsPrincipal principal)
        {
            var email = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            return (email == null) ? Task.FromResult<User>(null) : account.GetUserByEmailAsync(email.Value);
        }
    }
}
