
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
	public class CritterImageVariantController : Controller
    {
        private readonly LivestockContext _context;

        public CritterImageVariantController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.CritterImageVariant.Include(v => v.CritterImageModified).Include(v => v.CritterImageOriginal);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.CritterImageVariant.Include(v => v.CritterImageModified).Include(v => v.CritterImageOriginal).FirstOrDefaultAsync(m => m.CritterImageVariantId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[Authorize]
        public IActionResult Create()
        {
            ViewData["CritterImageModifiedId"] = new SelectList(_context.CritterImage, "CritterImageId", "CritterImageId");
ViewData["CritterImageOriginalId"] = new SelectList(_context.CritterImage, "CritterImageId", "CritterImageId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Create([Bind("CritterImageVariantId,CritterImageModifiedId,CritterImageOriginalId,Height,Timestamp,Width")]CritterImageVariant val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CritterImageModifiedId"] = new SelectList(_context.CritterImage, "CritterImageId", "CritterImageId", val.CritterImageModifiedId);
ViewData["CritterImageOriginalId"] = new SelectList(_context.CritterImage, "CritterImageId", "CritterImageId", val.CritterImageOriginalId);
            return View(val);
        }

		[Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.CritterImageVariant.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            ViewData["CritterImageModifiedId"] = new SelectList(_context.CritterImage, "CritterImageId", "CritterImageId", val.CritterImageModifiedId);
ViewData["CritterImageOriginalId"] = new SelectList(_context.CritterImage, "CritterImageId", "CritterImageId", val.CritterImageOriginalId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("CritterImageVariantId,CritterImageModifiedId,CritterImageOriginalId,Height,Timestamp,Width")]CritterImageVariant val)
        {
			if(val.CritterImageVariantId != id)
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
                    if (!Exists(val.CritterImageVariantId))
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
            ViewData["CritterImageModifiedId"] = new SelectList(_context.CritterImage, "CritterImageId", "CritterImageId", val.CritterImageModifiedId);
ViewData["CritterImageOriginalId"] = new SelectList(_context.CritterImage, "CritterImageId", "CritterImageId", val.CritterImageOriginalId);
            return View(val);
        }

		[Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.CritterImageVariant.Include(v => v.CritterImageModified).Include(v => v.CritterImageOriginal).FirstOrDefaultAsync(m => m.CritterImageVariantId == id);
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
            var val = await _context.CritterImageVariant.FindAsync(id);
            _context.CritterImageVariant.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(CritterImageVariant val)
		{
			
		}

        private bool Exists(int id)
        {
            return _context.CritterImageVariant.Any(e => e.CritterImageVariantId == id);
        }
    }
}