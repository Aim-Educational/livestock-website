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

    // This is here until we have a database version
    public class User
    {
        [DataType(DataType.EmailAddress)]
        public string Email;
        public string Role;
    }

    public interface IAccountInfoService
    {
        Task<User> GetUserByEmailAsync(string email);
        void AddClaimsFor(User user, ClaimsIdentity identity);
        void AddTemporaryUserClaims(ClaimsIdentity identity);
        bool HasPermission(User user, UserPermission permission);
    }

    public class AccountInfoService : IAccountInfoService
    {
        List<User> TEMP_DB = new List<User>()
        {
            new User{ Email = "sealabjaster@gmail.com", Role = "admin" },
            new User{ Email = "andyradford@hotmail.com", Role = "superadmin" }
        };

        public LivestockContext Db;

        public AccountInfoService(LivestockContext db)
        {
            this.Db = db;
        }

        public void AddClaimsFor(User user, ClaimsIdentity identity)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Email));
            identity.AddClaim(new Claim(ClaimTypes.Name, "TODO:Names"));
        }

        public void AddTemporaryUserClaims(ClaimsIdentity identity)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, "temp"));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "unknown@unknown.com"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "TODO:Names"));
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            // UNCOMMENT BELOW ONCE WE HAVE A DATABASE SOLUTION.
            //return this.Db.User.FirstOrDefaultAsync(user => user.Email == email);
            return Task.Run(() => TEMP_DB.FirstOrDefault(user => user.Email == email));
        }

        public bool HasPermission(User user, UserPermission permission)
        {
            // TODO: When we have a solid plan forward for this, actually do checks here.
            return true;
        }
    }

    public static class IAccountInfoServiceExtensions
    {
        public static Task<User> GetUserByPrincipleAsync(this IAccountInfoService account, ClaimsPrincipal principal)
        {
            var email = principal.FindFirst(c => c.Type == "email");
            return (email == null) ? Task.FromResult<User>(null) : account.GetUserByEmailAsync(email.Value);
        }
    }
}
