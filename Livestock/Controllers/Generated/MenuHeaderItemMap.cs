
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
	[AimAuthorize(RolesOR: "[Forbidden to all]")]
	public class MenuHeaderItemMapController : Controller
    {
        private readonly LivestockContext _context;

        public MenuHeaderItemMapController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.MenuHeaderItemMap.Include(v => v.MenuHeader).Include(v => v.MenuItem);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.MenuHeaderItemMap.Include(v => v.MenuHeader).Include(v => v.MenuItem).FirstOrDefaultAsync(m => m.MenuHeaderItemMapId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[AimAuthorize]
        public IActionResult Create()
        {
            ViewData["MenuHeaderId"] = new SelectList(_context.MenuHeader, "MenuHeaderId", "Name");
ViewData["MenuItemId"] = new SelectList(_context.MenuItem, "MenuItemId", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Create([Bind("MenuHeaderItemMapId,Comment,MenuHeaderId,MenuItemId,Timestamp,VersionNumber")]MenuHeaderItemMap val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuHeaderId"] = new SelectList(_context.MenuHeader, "MenuHeaderId", "Name", val.MenuHeaderId);
ViewData["MenuItemId"] = new SelectList(_context.MenuItem, "MenuItemId", "Title", val.MenuItemId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.MenuHeaderItemMap.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            ViewData["MenuHeaderId"] = new SelectList(_context.MenuHeader, "MenuHeaderId", "Name", val.MenuHeaderId);
ViewData["MenuItemId"] = new SelectList(_context.MenuItem, "MenuItemId", "Title", val.MenuItemId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Edit(int id, [Bind("MenuHeaderItemMapId,Comment,MenuHeaderId,MenuItemId,Timestamp,VersionNumber")]MenuHeaderItemMap val)
        {
			if(val.MenuHeaderItemMapId != id)
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
                    if (!Exists(val.MenuHeaderItemMapId))
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
            ViewData["MenuHeaderId"] = new SelectList(_context.MenuHeader, "MenuHeaderId", "Name", val.MenuHeaderId);
ViewData["MenuItemId"] = new SelectList(_context.MenuItem, "MenuItemId", "Title", val.MenuItemId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.MenuHeaderItemMap.Include(v => v.MenuHeader).Include(v => v.MenuItem).FirstOrDefaultAsync(m => m.MenuHeaderItemMapId == id);
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
            var val = await _context.MenuHeaderItemMap.FindAsync(id);
            _context.MenuHeaderItemMap.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(MenuHeaderItemMap val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.MenuHeaderItemMap.Any(e => e.MenuHeaderItemMapId == id);
        }
    }
}