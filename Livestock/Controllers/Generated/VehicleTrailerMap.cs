
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
	public class VehicleTrailerMapController : Controller
    {
        private readonly LivestockContext _context;

        public VehicleTrailerMapController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.VehicleTrailerMap.Include(v => v.Trailer).Include(v => v.VehicleMain);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.VehicleTrailerMap.Include(v => v.Trailer).Include(v => v.VehicleMain).FirstOrDefaultAsync(m => m.VehicleTrailerMapId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[AimAuthorize]
        public IActionResult Create()
        {
            ViewData["TrailerId"] = new SelectList(_context.Vehicle, "VehicleId", "Name");
ViewData["VehicleMainId"] = new SelectList(_context.Vehicle, "VehicleId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Create([Bind("VehicleTrailerMapId,Comment,Timestamp,TrailerId,VehicleMainId,VersionNumber")]VehicleTrailerMap val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrailerId"] = new SelectList(_context.Vehicle, "VehicleId", "Name", val.TrailerId);
ViewData["VehicleMainId"] = new SelectList(_context.Vehicle, "VehicleId", "Name", val.VehicleMainId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.VehicleTrailerMap.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            ViewData["TrailerId"] = new SelectList(_context.Vehicle, "VehicleId", "Name", val.TrailerId);
ViewData["VehicleMainId"] = new SelectList(_context.Vehicle, "VehicleId", "Name", val.VehicleMainId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Edit(int id, [Bind("VehicleTrailerMapId,Comment,Timestamp,TrailerId,VehicleMainId,VersionNumber")]VehicleTrailerMap val)
        {
			if(val.VehicleTrailerMapId != id)
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
                    if (!Exists(val.VehicleTrailerMapId))
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
            ViewData["TrailerId"] = new SelectList(_context.Vehicle, "VehicleId", "Name", val.TrailerId);
ViewData["VehicleMainId"] = new SelectList(_context.Vehicle, "VehicleId", "Name", val.VehicleMainId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.VehicleTrailerMap.Include(v => v.Trailer).Include(v => v.VehicleMain).FirstOrDefaultAsync(m => m.VehicleTrailerMapId == id);
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
            var val = await _context.VehicleTrailerMap.FindAsync(id);
            _context.VehicleTrailerMap.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(VehicleTrailerMap val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.VehicleTrailerMap.Any(e => e.VehicleTrailerMapId == id);
        }
    }
}