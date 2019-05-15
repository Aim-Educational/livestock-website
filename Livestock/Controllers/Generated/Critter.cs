
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
	[Authorize(Roles = "[Forbidden to all]")]
	public class CritterController : Controller
    {
        private readonly LivestockContext _context;

        public CritterController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.Critter.Include(v => v.Breed).Include(v => v.CritterImage).Include(v => v.CritterType).Include(v => v.DadCritter).Include(v => v.MumCritter).Include(v => v.OwnerContact);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Critter.Include(v => v.Breed).Include(v => v.CritterImage).Include(v => v.CritterType).Include(v => v.DadCritter).Include(v => v.MumCritter).Include(v => v.OwnerContact).FirstOrDefaultAsync(m => m.CritterId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[Authorize]
        public IActionResult Create()
        {
            ViewData["BreedId"] = new SelectList(_context.Breed, "BreedId", "Description");
ViewData["CritterImageId"] = new SelectList(_context.CritterImage, "CritterImageId", "CritterImageId");
ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Name");
ViewData["DadCritterId"] = new SelectList(_context.Critter, "CritterId", "CritterImageId");
ViewData["MumCritterId"] = new SelectList(_context.Critter, "CritterId", "CritterImageId");
ViewData["OwnerContactId"] = new SelectList(_context.Contact, "ContactId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Create([Bind("CritterId,BreedId,Comment,CritterImageId,CritterTypeId,DadCritterId,DadFurther,Gender,MumCritterId,MumFurther,Name,OwnerContactId,ReproduceFlags,Timestamp,VersionNumber")]Critter val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BreedId"] = new SelectList(_context.Breed, "BreedId", "Description", val.BreedId);
ViewData["CritterImageId"] = new SelectList(_context.CritterImage, "CritterImageId", "CritterImageId", val.CritterImageId);
ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Name", val.CritterTypeId);
ViewData["DadCritterId"] = new SelectList(_context.Critter, "CritterId", "CritterImageId", val.DadCritterId);
ViewData["MumCritterId"] = new SelectList(_context.Critter, "CritterId", "CritterImageId", val.MumCritterId);
ViewData["OwnerContactId"] = new SelectList(_context.Contact, "ContactId", "Name", val.OwnerContactId);
            return View(val);
        }

		[Authorize]
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
ViewData["CritterImageId"] = new SelectList(_context.CritterImage, "CritterImageId", "CritterImageId", val.CritterImageId);
ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Name", val.CritterTypeId);
ViewData["DadCritterId"] = new SelectList(_context.Critter, "CritterId", "CritterImageId", val.DadCritterId);
ViewData["MumCritterId"] = new SelectList(_context.Critter, "CritterId", "CritterImageId", val.MumCritterId);
ViewData["OwnerContactId"] = new SelectList(_context.Contact, "ContactId", "Name", val.OwnerContactId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("CritterId,BreedId,Comment,CritterImageId,CritterTypeId,DadCritterId,DadFurther,Gender,MumCritterId,MumFurther,Name,OwnerContactId,ReproduceFlags,Timestamp,VersionNumber")]Critter val)
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
ViewData["CritterImageId"] = new SelectList(_context.CritterImage, "CritterImageId", "CritterImageId", val.CritterImageId);
ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Name", val.CritterTypeId);
ViewData["DadCritterId"] = new SelectList(_context.Critter, "CritterId", "CritterImageId", val.DadCritterId);
ViewData["MumCritterId"] = new SelectList(_context.Critter, "CritterId", "CritterImageId", val.MumCritterId);
ViewData["OwnerContactId"] = new SelectList(_context.Contact, "ContactId", "Name", val.OwnerContactId);
            return View(val);
        }

		[Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Critter.Include(v => v.Breed).Include(v => v.CritterImage).Include(v => v.CritterType).Include(v => v.DadCritter).Include(v => v.MumCritter).Include(v => v.OwnerContact).FirstOrDefaultAsync(m => m.CritterId == id);
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