
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
	public class CritterController : Controller
    {
        private readonly LivestockContext _context;

        public CritterController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.Critter.Include(v => v.Breed).Include(v => v.CritterType).Include(v => v.DadCritter).Include(v => v.MumCritter);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Critter.Include(v => v.Breed).Include(v => v.CritterType).Include(v => v.DadCritter).Include(v => v.MumCritter).FirstOrDefaultAsync(m => m.CritterId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[AimAuthorize]
        public IActionResult Create()
        {
            ViewData["BreedId"] = new SelectList(_context.Breed, "BreedId", "Description");
ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Name");
ViewData["DadCritterId"] = new SelectList(_context.Critter, "CritterId", "Name");
ViewData["MumCritterId"] = new SelectList(_context.Critter, "CritterId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Create([Bind("CritterId,BreedId,Comment,CritterTypeId,DadCritterId,DadFurther,Gender,MumCritterId,MumFurther,Name,OwnerContactId,Timestamp,VersionNumber")]Critter val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BreedId"] = new SelectList(_context.Breed, "BreedId", "Description", val.BreedId);
ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Name", val.CritterTypeId);
ViewData["DadCritterId"] = new SelectList(_context.Critter, "CritterId", "Name", val.DadCritterId);
ViewData["MumCritterId"] = new SelectList(_context.Critter, "CritterId", "Name", val.MumCritterId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Critter.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            ViewData["BreedId"] = new SelectList(_context.Breed, "BreedId", "Description", val.BreedId);
ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Name", val.CritterTypeId);
ViewData["DadCritterId"] = new SelectList(_context.Critter, "CritterId", "Name", val.DadCritterId);
ViewData["MumCritterId"] = new SelectList(_context.Critter, "CritterId", "Name", val.MumCritterId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Edit(int id, [Bind("CritterId,BreedId,Comment,CritterTypeId,DadCritterId,DadFurther,Gender,MumCritterId,MumFurther,Name,OwnerContactId,Timestamp,VersionNumber")]Critter val)
        {
			if(val.CritterId != id)
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
                    if (!Exists(val.CritterId))
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
            ViewData["BreedId"] = new SelectList(_context.Breed, "BreedId", "Description", val.BreedId);
ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Name", val.CritterTypeId);
ViewData["DadCritterId"] = new SelectList(_context.Critter, "CritterId", "Name", val.DadCritterId);
ViewData["MumCritterId"] = new SelectList(_context.Critter, "CritterId", "Name", val.MumCritterId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Critter.Include(v => v.Breed).Include(v => v.CritterType).Include(v => v.DadCritter).Include(v => v.MumCritter).FirstOrDefaultAsync(m => m.CritterId == id);
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
            var val = await _context.Critter.FindAsync(id);
            _context.Critter.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(Critter val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
if(String.IsNullOrWhiteSpace(val.DadFurther)) val.DadFurther = "N/A";
if(String.IsNullOrWhiteSpace(val.Gender)) val.Gender = "?";
if(String.IsNullOrWhiteSpace(val.MumFurther)) val.MumFurther = "N/A";
if(String.IsNullOrWhiteSpace(val.Name)) val.Name = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.Critter.Any(e => e.CritterId == id);
        }
    }
}