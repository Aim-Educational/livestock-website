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
                        GroupMemberCount = this.GetMappingInfo(g).Count()
                    })
                }
            );
        }

        public IActionResult Create([FromQuery] string type)
        {
            var info = this.GetGroupInfo(type);
            ViewData["GroupDescriptions"] = info.ToDictionary(i => i.Id, i => i.Description);
            return View(
                "CreateEdit",
                new GroupCreateEditViewModel
                {
                    Group = null,
                    SelectedMemberIds = null,
                    GroupType = type,
                    CreateOrEdit = "create"
                }
            );
        }

        public IActionResult Edit([FromRoute] int id)
        {
            var group = this.livestockDb.AdmuGroup.Find(id);
            var type = (group.GroupType == AdmuGroupEntityTypes.Critter) ? "critter" : "user";
            var info = this.GetGroupInfo(type);

            ViewData["GroupDescriptions"] = info.ToDictionary(i => i.Id, i => i.Description);
            return View(
                "CreateEdit",
                new GroupCreateEditViewModel
                {
                    Group = group,
                    SelectedMemberIds = this.GetMappingInfo(group).Select(i => i.DataId),
                    GroupType = type,
                    CreateOrEdit = "edit"
                }
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupCreateEditViewModel model)
        {
            if(!ModelState.IsValid)
            {
                var info = this.GetGroupInfo(model.GroupType);
                ViewData["GroupDescriptions"] = info.ToDictionary(i => i.Id, i => i.Description);
                return View("CreateEdit", model);
            }

            this.livestockDb.AdmuGroup.Add(model.Group);
            await this.livestockDb.SaveChangesAsync();
            await this.AddToGroup(model.Group, model.SelectedMemberIds);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GroupCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var info = this.GetGroupInfo(model.GroupType);
                ViewData["GroupDescriptions"] = info.ToDictionary(i => i.Id, i => i.Description);
                return View("CreateEdit", model);
            }

            this.livestockDb.Update(model.Group);            

            // First, remove any that no longer are needed.
            var maps = this.GetMappingInfo(model.Group);
            var ids = model.SelectedMemberIds.ToList(); // We need it as a list so we can use .Remove
            foreach(var id in maps.Select(m => m.DataId).ToList()) // We're removing things, so we need to cache the data in a list first.
            {
                if(!ids.Contains(id))
                {
                    if(model.Group.GroupType == AdmuGroupEntityTypes.Critter)
                        await this.data.MultiReference<Critter>().RemoveByIdAsync(model.Group, id);
                    else if(model.Group.GroupType == AdmuGroupEntityTypes.User)
                        await this.data.MultiReference<User>().RemoveByIdAsync(model.Group, id);
                    else
                        throw new InvalidOperationException();
                }

                // Remove the id from the list, since we either deleted it, or we're not changing it at all.
                ids.Remove(id);
            }

            // Finally, add in anything new.
            await this.AddToGroup(model.Group, ids);
            await this.livestockDb.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private async Task AddToGroup(AdmuGroup group, IEnumerable<int> items)
        {
            foreach (var id in items)
            {
                // This is slow, but at our current and future scales, it won't really matter.
                if (group.GroupType == AdmuGroupEntityTypes.Critter)
                    await this.data.MultiReference<Critter>().AddAsync(group, this.livestockDb.Critter.Find(id));
                else if (group.GroupType == AdmuGroupEntityTypes.User)
                    await this.data.MultiReference<User>().AddAsync(group, this.loginDb.Users.Find(id));
                else
                    throw new InvalidOperationException();
            }
        }

        private IQueryable<MappingInfo> GetMappingInfo(AdmuGroup group)
        {
            return (group.GroupType == AdmuGroupEntityTypes.Critter)
                   ? this.data.MultiReference<Critter>().GetAllMappingInfoForSingle(group)
                   : this.data.MultiReference<User>().GetAllMappingInfoForSingle(group);
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