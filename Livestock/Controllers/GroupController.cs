using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aim.DataMapper;
using AimLogin.DbModel;
using Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Models;
using Website.Other;

namespace Website.Controllers
{
    [Authorize(Roles = "staff,admin")]
    public class GroupController : Controller
    {
        readonly AimLoginContext loginDb;
        readonly LivestockContext livestockDb;
        readonly DataMapService<AdmuGroup> data;
        readonly DataMapService<User> userData;

        public GroupController(AimLoginContext loginDb, LivestockContext livestockDb, DataMapService<AdmuGroup> data, DataMapService<User> userData)
        {
            this.loginDb = loginDb;
            this.livestockDb = livestockDb;
            this.data = data;
            this.userData = userData;
        }

        public IActionResult Index()
        {
            return View(
                new GroupIndexViewModel
                {
                    Groups = this.livestockDb.AdmuGroup.ToList().Select(g => new GroupIndexInfo
                    {
                        Group = g,
                        GroupMemberCount = (g.GroupType == AdmuGroupEntityTypes.Critter)
                                           ? this.data.MultiReference<Critter>().GetAllMappingInfoForSingle(g).Count()
                                           : this.data.MultiReference<User>().GetAllMappingInfoForSingle(g).Count()
                    })
                }
            );
        }

        public IActionResult Create([FromQuery] string type)
        {
            var info = this.GetGroupInfo(type);
            ViewData["GroupDescriptions"] = info.ToDictionary(i => i.Id, i => i.Description);
            return View(
                new GroupCreateViewModel
                {
                    Group = null,
                    SelectedGroupIds = null,
                    GroupType = type
                }
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupCreateViewModel model)
        {
            if(!ModelState.IsValid)
            {
                var info = this.GetGroupInfo(model.GroupType);
                ViewData["GroupDescriptions"] = info.ToDictionary(i => i.Id, i => i.Description);
                return View(model);
            }

            this.livestockDb.AdmuGroup.Add(model.Group);
            await this.livestockDb.SaveChangesAsync();
            
            foreach(var id in model.SelectedGroupIds)
            {
                // This is slow, but at our current and future scales, it won't really matter.
                if(model.GroupType == "critter")
                    await this.data.MultiReference<Critter>().AddAsync(model.Group, this.livestockDb.Critter.Find(id));
                else if(model.GroupType == "user")
                    await this.data.MultiReference<User>().AddAsync(model.Group, this.loginDb.Users.Find(id));
                else
                    throw new InvalidOperationException();
            }

            return RedirectToAction("Index");
        }

        private IEnumerable<GroupInfo> GetGroupInfo(string type)
        {
            switch(type)
            {
                case "critter":
                    return this.livestockDb.Critter.Select(c => new GroupInfo{ Id = c.CritterId, Description = c.Name }).AsEnumerable();

                case "user":
                    return this.loginDb.Users.AsEnumerable().Select(u => new GroupInfo
                    {
                        Id = u.UserId, 
                        // TODO: Reduce this into a single call to SingleValue, instead of two.
                        Description = this.userData.SingleValue<AlUserInfo>().GetOrDefaultAsync(u).Result.FirstName
                                    + " "
                                    + this.userData.SingleValue<AlUserInfo>().GetOrDefaultAsync(u).Result.LastName
                    });

                default:
                    throw new InvalidOperationException($"No type called '{type}'");
            }
        }
    }

    class GroupInfo
    {
        public string Description;
        public int Id;
    }
}