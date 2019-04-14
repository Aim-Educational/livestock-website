
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
	public class CritterTypeController : Controller
    {
        private readonly LivestockContext _context;

        public CritterTypeController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.CritterType;
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.CritterType.FirstOrDefaultAsync(m => m.CritterTypeId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[Authorize]
        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Create([Bind("CritterTypeId,Comment,GestrationPeriod,Name,Timestamp,VersionNumber")]CritterType val)
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

		[Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.CritterType.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("CritterTypeId,Comment,GestrationPeriod,Name,Timestamp,VersionNumber")]CritterType val)
        {
			if(val.CritterTypeId != id)
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
                    if (!Exists(val.CritterTypeId))
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

		[Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.CritterType.FirstOrDefaultAsync(m => m.CritterTypeId == id);
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
            var val = await _context.CritterType.FindAsync(id);
            _context.CritterType.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(CritterType val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
if(String.IsNullOrWhiteSpace(val.Name)) val.Name = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.CritterType.Any(e => e.CritterTypeId == id);
        }
    }
}