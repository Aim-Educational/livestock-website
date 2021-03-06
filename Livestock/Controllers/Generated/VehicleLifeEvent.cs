
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Microsoft.AspNetCore.Authorization;

namespace Website.Controllers
{
	[Authorize(Roles = "admin,")]
	public class VehicleLifeEventController : Controller
    {
        private readonly LivestockContext _context;

        public VehicleLifeEventController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.VehicleLifeEvent.Include(v => v.EnumVehicleLifeEventType).Include(v => v.VehicleTrailerMap);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.VehicleLifeEvent.Include(v => v.EnumVehicleLifeEventType).Include(v => v.VehicleTrailerMap).FirstOrDefaultAsync(m => m.VehicleLifeEventId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[Authorize]
        public IActionResult Create()
        {
            ViewData["EnumVehicleLifeEventTypeId"] = new SelectList(_context.EnumVehicleLifeEventType, "EnumVehicleLifeEventTypeId", "Description");
ViewData["VehicleTrailerMapId"] = new SelectList(_context.VehicleTrailerMap, "VehicleTrailerMapId", "VehicleTrailerMapId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Create([Bind("VehicleLifeEventId,Comment,DateTime,Description,EnumVehicleLifeEventTypeId,Timestamp,VehicleTrailerMapId,VersionNumber")]VehicleLifeEvent val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnumVehicleLifeEventTypeId"] = new SelectList(_context.EnumVehicleLifeEventType, "EnumVehicleLifeEventTypeId", "Description", val.EnumVehicleLifeEventTypeId);
ViewData["VehicleTrailerMapId"] = new SelectList(_context.VehicleTrailerMap, "VehicleTrailerMapId", "VehicleTrailerMapId", val.VehicleTrailerMapId);
            return View(val);
        }

		[Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.VehicleLifeEvent.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            ViewData["EnumVehicleLifeEventTypeId"] = new SelectList(_context.EnumVehicleLifeEventType, "EnumVehicleLifeEventTypeId", "Description", val.EnumVehicleLifeEventTypeId);
ViewData["VehicleTrailerMapId"] = new SelectList(_context.VehicleTrailerMap, "VehicleTrailerMapId", "VehicleTrailerMapId", val.VehicleTrailerMapId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("VehicleLifeEventId,Comment,DateTime,Description,EnumVehicleLifeEventTypeId,Timestamp,VehicleTrailerMapId,VersionNumber")]VehicleLifeEvent val)
        {
			if(val.VehicleLifeEventId != id)
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
                    if (!Exists(val.VehicleLifeEventId))
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
            ViewData["EnumVehicleLifeEventTypeId"] = new SelectList(_context.EnumVehicleLifeEventType, "EnumVehicleLifeEventTypeId", "Description", val.EnumVehicleLifeEventTypeId);
ViewData["VehicleTrailerMapId"] = new SelectList(_context.VehicleTrailerMap, "VehicleTrailerMapId", "VehicleTrailerMapId", val.VehicleTrailerMapId);
            return View(val);
        }

		[Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.VehicleLifeEvent.Include(v => v.EnumVehicleLifeEventType).Include(v => v.VehicleTrailerMap).FirstOrDefaultAsync(m => m.VehicleLifeEventId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var val = await _context.VehicleLifeEvent.FindAsync(id);
            _context.VehicleLifeEvent.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(VehicleLifeEvent val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
if(String.IsNullOrWhiteSpace(val.Description)) val.Description = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.VehicleLifeEvent.Any(e => e.VehicleLifeEventId == id);
        }
    }
}