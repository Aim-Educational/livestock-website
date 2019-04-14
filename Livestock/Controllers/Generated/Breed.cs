
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
	public class BreedController : Controller
    {
        private readonly LivestockContext _context;

        public BreedController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.Breed.Include(v => v.BreedSocietyContact).Include(v => v.CritterType);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Breed.Include(v => v.BreedSocietyContact).Include(v => v.CritterType).FirstOrDefaultAsync(m => m.BreedId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[Authorize]
        public IActionResult Create()
        {
            ViewData["BreedSocietyContactId"] = new SelectList(_context.Contact, "ContactId", "Name");
ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Create([Bind("BreedId,BreedSocietyContactId,Comment,CritterTypeId,Description,Registerable,Timestamp,VersionNumber")]Breed val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BreedSocietyContactId"] = new SelectList(_context.Contact, "ContactId", "Name", val.BreedSocietyContactId);
ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Name", val.CritterTypeId);
            return View(val);
        }

		[Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Breed.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            ViewData["BreedSocietyContactId"] = new SelectList(_context.Contact, "ContactId", "Name", val.BreedSocietyContactId);
ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Name", val.CritterTypeId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("BreedId,BreedSocietyContactId,Comment,CritterTypeId,Description,Registerable,Timestamp,VersionNumber")]Breed val)
        {
			if(val.BreedId != id)
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
                    if (!Exists(val.BreedId))
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
            ViewData["BreedSocietyContactId"] = new SelectList(_context.Contact, "ContactId", "Name", val.BreedSocietyContactId);
ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Name", val.CritterTypeId);
            return View(val);
        }

		[Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Breed.Include(v => v.BreedSocietyContact).Include(v => v.CritterType).FirstOrDefaultAsync(m => m.BreedId == id);
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
            var val = await _context.Breed.FindAsync(id);
            _context.Breed.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(Breed val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
if(String.IsNullOrWhiteSpace(val.Description)) val.Description = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.Breed.Any(e => e.BreedId == id);
        }
    }
}