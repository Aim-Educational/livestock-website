
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
	public class MenuItemController : Controller
    {
        private readonly LivestockContext _context;

        public MenuItemController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.MenuItem;
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.MenuItem.FirstOrDefaultAsync(m => m.MenuItemId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[AimAuthorize]
        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Create([Bind("MenuItemId,Action,Comment,Controller,IconUri,RoleId,SequenceNumber,Timestamp,Title,VersionNumber")]MenuItem val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.MenuItem.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Edit(int id, [Bind("MenuItemId,Action,Comment,Controller,IconUri,RoleId,SequenceNumber,Timestamp,Title,VersionNumber")]MenuItem val)
        {
			if(val.MenuItemId != id)
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
                    if (!Exists(val.MenuItemId))
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
            
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.MenuItem.FirstOrDefaultAsync(m => m.MenuItemId == id);
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
            var val = await _context.MenuItem.FindAsync(id);
            _context.MenuItem.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(MenuItem val)
		{
			if(String.IsNullOrWhiteSpace(val.Action)) val.Action = "N/A";
if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
if(String.IsNullOrWhiteSpace(val.Controller)) val.Controller = "N/A";
if(String.IsNullOrWhiteSpace(val.IconUri)) val.IconUri = "N/A";
if(String.IsNullOrWhiteSpace(val.Title)) val.Title = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.MenuItem.Any(e => e.MenuItemId == id);
        }
    }
}