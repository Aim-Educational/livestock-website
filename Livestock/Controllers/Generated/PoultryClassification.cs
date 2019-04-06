
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
	public class PoultryClassificationController : Controller
    {
        private readonly LivestockContext _context;

        public PoultryClassificationController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.PoultryClassification.Include(v => v.CritterType);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.PoultryClassification.Include(v => v.CritterType).FirstOrDefaultAsync(m => m.PoultryClassificationId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[Authorize]
        public IActionResult Create()
        {
            ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Create([Bind("PoultryClassificationId,Comment,CritterTypeId,Timestamp,VersionNumber")]PoultryClassification val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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

            var val = await _context.PoultryClassification.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Name", val.CritterTypeId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("PoultryClassificationId,Comment,CritterTypeId,Timestamp,VersionNumber")]PoultryClassification val)
        {
			if(val.PoultryClassificationId != id)
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
                    if (!Exists(val.PoultryClassificationId))
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

            var val = await _context.PoultryClassification.Include(v => v.CritterType).FirstOrDefaultAsync(m => m.PoultryClassificationId == id);
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
            var val = await _context.PoultryClassification.FindAsync(id);
            _context.PoultryClassification.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(PoultryClassification val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.PoultryClassification.Any(e => e.PoultryClassificationId == id);
        }
    }
}