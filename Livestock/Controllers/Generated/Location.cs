
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
	[AimAuthorize(RolesOR: "admin,")]
	public class LocationController : Controller
    {
        private readonly LivestockContext _context;

        public LocationController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.Location.Include(v => v.EnumLocationType);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Location.Include(v => v.EnumLocationType).FirstOrDefaultAsync(m => m.LocationId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[AimAuthorize]
        public IActionResult Create()
        {
            ViewData["EnumLocationTypeId"] = new SelectList(_context.EnumLocationType, "EnumLocationTypeId", "Description");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Create([Bind("LocationId,Comment,EnumLocationTypeId,HoldingId,Name,ParentId,Timestamp,VersionNumber")]Location val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnumLocationTypeId"] = new SelectList(_context.EnumLocationType, "EnumLocationTypeId", "Description", val.EnumLocationTypeId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Location.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            ViewData["EnumLocationTypeId"] = new SelectList(_context.EnumLocationType, "EnumLocationTypeId", "Description", val.EnumLocationTypeId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Edit(int id, [Bind("LocationId,Comment,EnumLocationTypeId,HoldingId,Name,ParentId,Timestamp,VersionNumber")]Location val)
        {
			if(val.LocationId != id)
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
                    if (!Exists(val.LocationId))
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
            ViewData["EnumLocationTypeId"] = new SelectList(_context.EnumLocationType, "EnumLocationTypeId", "Description", val.EnumLocationTypeId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Location.Include(v => v.EnumLocationType).FirstOrDefaultAsync(m => m.LocationId == id);
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
            var val = await _context.Location.FindAsync(id);
            _context.Location.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(Location val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
if(String.IsNullOrWhiteSpace(val.Name)) val.Name = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.Location.Any(e => e.LocationId == id);
        }
    }
}