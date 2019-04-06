
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
	public class TagController : Controller
    {
        private readonly LivestockContext _context;

        public TagController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.Tag.Include(v => v.Critter);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Tag.Include(v => v.Critter).FirstOrDefaultAsync(m => m.TagId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[Authorize]
        public IActionResult Create()
        {
            ViewData["CritterId"] = new SelectList(_context.Critter, "CritterId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Create([Bind("TagId,Comment,CritterId,DateTime,Rfid,Tag1,Timestamp,UserId,VersionNumber")]Tag val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CritterId"] = new SelectList(_context.Critter, "CritterId", "Name", val.CritterId);
            return View(val);
        }

		[Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Tag.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            ViewData["CritterId"] = new SelectList(_context.Critter, "CritterId", "Name", val.CritterId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("TagId,Comment,CritterId,DateTime,Rfid,Tag1,Timestamp,UserId,VersionNumber")]Tag val)
        {
			if(val.TagId != id)
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
                    if (!Exists(val.TagId))
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
            return View(val);
        }

		[Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Tag.Include(v => v.Critter).FirstOrDefaultAsync(m => m.TagId == id);
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
            var val = await _context.Tag.FindAsync(id);
            _context.Tag.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(Tag val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
if(String.IsNullOrWhiteSpace(val.Rfid)) val.Rfid = "N/A";
if(String.IsNullOrWhiteSpace(val.Tag1)) val.Tag1 = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.Tag.Any(e => e.TagId == id);
        }
    }
}