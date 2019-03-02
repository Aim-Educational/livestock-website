
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
	public class HoldingController : Controller
    {
        private readonly LivestockContext _context;

        public HoldingController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.Holding.Include(v => v.Contact);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Holding.Include(v => v.Contact).FirstOrDefaultAsync(m => m.HoldingId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[AimAuthorize]
        public IActionResult Create()
        {
            ViewData["ContactId"] = new SelectList(_context.Contact, "ContactId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Create([Bind("HoldingId,Address,Comment,ContactId,GridReference,HoldingNumber,Postcode,RegisterForCows,RegisterForFish,RegisterForPigs,RegisterForPoultry,RegisterForSheepGoats,Timestamp,VersionNumber")]Holding val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContactId"] = new SelectList(_context.Contact, "ContactId", "Name", val.ContactId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Holding.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            ViewData["ContactId"] = new SelectList(_context.Contact, "ContactId", "Name", val.ContactId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Edit(int id, [Bind("HoldingId,Address,Comment,ContactId,GridReference,HoldingNumber,Postcode,RegisterForCows,RegisterForFish,RegisterForPigs,RegisterForPoultry,RegisterForSheepGoats,Timestamp,VersionNumber")]Holding val)
        {
			if(val.HoldingId != id)
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
                    if (!Exists(val.HoldingId))
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
            ViewData["ContactId"] = new SelectList(_context.Contact, "ContactId", "Name", val.ContactId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Holding.Include(v => v.Contact).FirstOrDefaultAsync(m => m.HoldingId == id);
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
            var val = await _context.Holding.FindAsync(id);
            _context.Holding.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(Holding val)
		{
			if(String.IsNullOrWhiteSpace(val.Address)) val.Address = "N/A";
if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
if(String.IsNullOrWhiteSpace(val.GridReference)) val.GridReference = "N/A";
if(String.IsNullOrWhiteSpace(val.HoldingNumber)) val.HoldingNumber = "N/A";
if(String.IsNullOrWhiteSpace(val.Postcode)) val.Postcode = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.Holding.Any(e => e.HoldingId == id);
        }
    }
}