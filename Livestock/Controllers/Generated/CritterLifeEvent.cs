
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
	[AimAuthorize(RolesOR: "admin,student,")]
	public class CritterLifeEventController : Controller
    {
        private readonly LivestockContext _context;

        public CritterLifeEventController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.CritterLifeEvent.Include(v => v.Critter).Include(v => v.EnumCritterLifeEventType);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.CritterLifeEvent.Include(v => v.Critter).Include(v => v.EnumCritterLifeEventType).FirstOrDefaultAsync(m => m.CritterLifeEventId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[AimAuthorize]
        public IActionResult Create()
        {
            ViewData["CritterId"] = new SelectList(_context.Critter, "CritterId", "Name");
ViewData["EnumCritterLifeEventTypeId"] = new SelectList(_context.EnumCritterLifeEventType, "EnumCritterLifeEventTypeId", "Description");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Create([Bind("CritterLifeEventId,Comment,CritterId,DateTime,Description,EnumCritterLifeEventTypeId,Timestamp,VersionNumber")]CritterLifeEvent val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CritterId"] = new SelectList(_context.Critter, "CritterId", "Name", val.CritterId);
ViewData["EnumCritterLifeEventTypeId"] = new SelectList(_context.EnumCritterLifeEventType, "EnumCritterLifeEventTypeId", "Description", val.EnumCritterLifeEventTypeId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.CritterLifeEvent.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            ViewData["CritterId"] = new SelectList(_context.Critter, "CritterId", "Name", val.CritterId);
ViewData["EnumCritterLifeEventTypeId"] = new SelectList(_context.EnumCritterLifeEventType, "EnumCritterLifeEventTypeId", "Description", val.EnumCritterLifeEventTypeId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Edit(int id, [Bind("CritterLifeEventId,Comment,CritterId,DateTime,Description,EnumCritterLifeEventTypeId,Timestamp,VersionNumber")]CritterLifeEvent val)
        {
			if(val.CritterLifeEventId != id)
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
                    if (!Exists(val.CritterLifeEventId))
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
            ViewData["CritterId"] = new SelectList(_context.Critter, "CritterId", "Name", val.CritterId);
ViewData["EnumCritterLifeEventTypeId"] = new SelectList(_context.EnumCritterLifeEventType, "EnumCritterLifeEventTypeId", "Description", val.EnumCritterLifeEventTypeId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.CritterLifeEvent.Include(v => v.Critter).Include(v => v.EnumCritterLifeEventType).FirstOrDefaultAsync(m => m.CritterLifeEventId == id);
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
            var val = await _context.CritterLifeEvent.FindAsync(id);
            _context.CritterLifeEvent.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(CritterLifeEvent val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
if(String.IsNullOrWhiteSpace(val.Description)) val.Description = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.CritterLifeEvent.Any(e => e.CritterLifeEventId == id);
        }
    }
}