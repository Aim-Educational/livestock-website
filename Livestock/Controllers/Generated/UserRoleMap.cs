
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Website.Filters;
using Website.Services;
using User = Database.Models.User;

namespace Website.Controllers
{
	[AimAuthorize(RolesOR: "admin")]
	public class UserRoleMapController : Controller
    {
        private readonly LivestockContext _context;

        public UserRoleMapController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.UserRoleMap.Include(v => v.Role).Include(v => v.User);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.UserRoleMap.Include(v => v.Role).Include(v => v.User).FirstOrDefaultAsync(m => m.UserRoleMapId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[AimAuthorize]
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "Description");
ViewData["UserId"] = new SelectList(_context.User, "UserId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Create([Bind("UserRoleMapId,Comment,RoleId,Timestamp,UserId,VersionNumber")]UserRoleMap val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "Description", val.RoleId);
ViewData["UserId"] = new SelectList(_context.User, "UserId", "Name", val.UserId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.UserRoleMap.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "Description", val.RoleId);
ViewData["UserId"] = new SelectList(_context.User, "UserId", "Name", val.UserId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Edit(int id, [Bind("UserRoleMapId,Comment,RoleId,Timestamp,UserId,VersionNumber")]UserRoleMap val)
        {
			if(val.UserRoleMapId != id)
				return NotFound();

			this.FixNullFields(val);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(val);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Exists(val.UserRoleMapId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "Description", val.RoleId);
ViewData["UserId"] = new SelectList(_context.User, "UserId", "Name", val.UserId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.UserRoleMap.Include(v => v.Role).Include(v => v.User).FirstOrDefaultAsync(m => m.UserRoleMapId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var val = await _context.UserRoleMap.FindAsync(id);
            _context.UserRoleMap.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(UserRoleMap val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.UserRoleMap.Any(e => e.UserRoleMapId == id);
        }
    }
}