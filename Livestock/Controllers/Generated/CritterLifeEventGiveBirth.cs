
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
	public class CritterLifeEventGiveBirthController : Controller
    {
        private readonly LivestockContext _context;

        public CritterLifeEventGiveBirthController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.CritterLifeEventGiveBirth;
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.CritterLifeEventGiveBirth.FirstOrDefaultAsync(m => m.CritterLifeEventGiveBirthId == id);
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
        public async Task<IActionResult> Create([Bind("CritterLifeEventGiveBirthId,Comment,DateTime,Timestamp,VersionNumber")]CritterLifeEventGiveBirth val)
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

            var val = await _context.CritterLifeEventGiveBirth.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Edit(int id, [Bind("CritterLifeEventGiveBirthId,Comment,DateTime,Timestamp,VersionNumber")]CritterLifeEventGiveBirth val)
        {
			if(val.CritterLifeEventGiveBirthId != id)
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
                    if (!Exists(val.CritterLifeEventGiveBirthId))
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

            var val = await _context.CritterLifeEventGiveBirth.FirstOrDefaultAsync(m => m.CritterLifeEventGiveBirthId == id);
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
            var val = await _context.CritterLifeEventGiveBirth.FindAsync(id);
            _context.CritterLifeEventGiveBirth.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(CritterLifeEventGiveBirth val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.CritterLifeEventGiveBirth.Any(e => e.CritterLifeEventGiveBirthId == id);
        }
    }
}