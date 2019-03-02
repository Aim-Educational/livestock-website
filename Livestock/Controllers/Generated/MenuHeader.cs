
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
	public class MenuHeaderController : Controller
    {
        private readonly LivestockContext _context;

        public MenuHeaderController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.MenuHeader.Include(v => v.MenuHeaderParent).Include(v => v.Role);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.MenuHeader.Include(v => v.MenuHeaderParent).Include(v => v.Role).FirstOrDefaultAsync(m => m.MenuHeaderId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[AimAuthorize]
        public IActionResult Create()
        {
            ViewData["MenuHeaderParentId"] = new SelectList(_context.MenuHeader, "MenuHeaderId", "Name");
ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "Description");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Create([Bind("MenuHeaderId,ApplicationCode,Comment,ImageUri,MenuHeaderParentId,Name,RoleId,Timestamp,Title,VersionNumber")]MenuHeader val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuHeaderParentId"] = new SelectList(_context.MenuHeader, "MenuHeaderId", "Name", val.MenuHeaderParentId);
ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "Description", val.RoleId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.MenuHeader.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            ViewData["MenuHeaderParentId"] = new SelectList(_context.MenuHeader, "MenuHeaderId", "Name", val.MenuHeaderParentId);
ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "Description", val.RoleId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Edit(int id, [Bind("MenuHeaderId,ApplicationCode,Comment,ImageUri,MenuHeaderParentId,Name,RoleId,Timestamp,Title,VersionNumber")]MenuHeader val)
        {
			if(val.MenuHeaderId != id)
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
                    if (!Exists(val.MenuHeaderId))
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
            ViewData["MenuHeaderParentId"] = new SelectList(_context.MenuHeader, "MenuHeaderId", "Name", val.MenuHeaderParentId);
ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "Description", val.RoleId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.MenuHeader.Include(v => v.MenuHeaderParent).Include(v => v.Role).FirstOrDefaultAsync(m => m.MenuHeaderId == id);
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
            var val = await _context.MenuHeader.FindAsync(id);
            _context.MenuHeader.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(MenuHeader val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
if(String.IsNullOrWhiteSpace(val.ImageUri)) val.ImageUri = "N/A";
if(String.IsNullOrWhiteSpace(val.Name)) val.Name = "N/A";
if(String.IsNullOrWhiteSpace(val.Title)) val.Title = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.MenuHeader.Any(e => e.MenuHeaderId == id);
        }
    }
}