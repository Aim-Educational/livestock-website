using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aim.DataMapper;
using AimLogin.DbModel;
using Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public GroupController(AimLoginContext loginDb, LivestockContext livestockDb, DataMapService<AdmuGroup> data)
        {
            this.loginDb = loginDb;
            this.livestockDb = livestockDb;
            this.data = data;
        }

        public IActionResult Index()
        {
            ViewData["TEMP_Names"] = new List<string>{ "One", "Two", "Three" };
            return View(
                new GroupIndexViewModel
                {
                    Groups = this.livestockDb.AdmuGroup.ToList().Select(g => new GroupIndexInfo
                    {
                        Group = g,
                        GroupMemberCount = (g.GroupType == AdmuGroupEntityTypes.Critter)
                                           ? this.data.MultiReference<Critter>().GetAllMappingInfoForSingle(g).Count()
                                           : this.data.MultiReference<User>().GetAllMappingInfoForSingle(g).Count()
                    }),
                    TEMP = new List<int>{ 1, 2 }
                }
            );
        }
    }
}