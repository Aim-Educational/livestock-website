using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Models;

namespace Website.Controllers
{
    public class CrittersController : Controller
    {
        private readonly LivestockContext _context;

        public CrittersController(LivestockContext context)
        {
            _context = context;
        }

        // GET: Critters
        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.Critter.Include(c => c.CritterType).Include(c => c.DadCritter).Include(c => c.MumCritter);
            return View(await livestockContext.ToListAsync());
        }

        // GET: Critters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var critter = await _context.Critter
                .Include(c => c.CritterType)
                .Include(c => c.DadCritter)
                .Include(c => c.MumCritter)
                .FirstOrDefaultAsync(m => m.CritterId == id);
            if (critter == null)
            {
                return NotFound();
            }

            return View(critter);
        }

        // GET: Critters/Create
        public IActionResult Create()
        {
            ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Comment");
            ViewData["DadCritterId"] = new SelectList(_context.Critter, "CritterId", "Comment");
            ViewData["MumCritterId"] = new SelectList(_context.Critter, "CritterId", "Comment");
            return View();
        }

        // POST: Critters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CritterId,CritterTypeId,Gender,Name,MumCritterId,DadCritterId,MumFurther,DadFurther,OwnerContactId,BreedId,Comment,Timestamp,VersionNumber")] Critter critter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(critter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Comment", critter.CritterTypeId);
            ViewData["DadCritterId"] = new SelectList(_context.Critter, "CritterId", "Comment", critter.DadCritterId);
            ViewData["MumCritterId"] = new SelectList(_context.Critter, "CritterId", "Comment", critter.MumCritterId);
            return View(critter);
        }

        // GET: Critters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var critter = await _context.Critter.FindAsync(id);
            if (critter == null)
            {
                return NotFound();
            }
            ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Comment", critter.CritterTypeId);
            ViewData["DadCritterId"] = new SelectList(_context.Critter, "CritterId", "Comment", critter.DadCritterId);
            ViewData["MumCritterId"] = new SelectList(_context.Critter, "CritterId", "Comment", critter.MumCritterId);
            return View(critter);
        }

        // POST: Critters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CritterId,CritterTypeId,Gender,Name,MumCritterId,DadCritterId,MumFurther,DadFurther,OwnerContactId,BreedId,Comment,Timestamp,VersionNumber")] Critter critter)
        {
            if (id != critter.CritterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(critter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CritterExists(critter.CritterId))
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
            ViewData["CritterTypeId"] = new SelectList(_context.CritterType, "CritterTypeId", "Comment", critter.CritterTypeId);
            ViewData["DadCritterId"] = new SelectList(_context.Critter, "CritterId", "Comment", critter.DadCritterId);
            ViewData["MumCritterId"] = new SelectList(_context.Critter, "CritterId", "Comment", critter.MumCritterId);
            return View(critter);
        }

        // GET: Critters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var critter = await _context.Critter
                .Include(c => c.CritterType)
                .Include(c => c.DadCritter)
                .Include(c => c.MumCritter)
                .FirstOrDefaultAsync(m => m.CritterId == id);
            if (critter == null)
            {
                return NotFound();
            }

            return View(critter);
        }

        // POST: Critters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var critter = await _context.Critter.FindAsync(id);
            _context.Critter.Remove(critter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CritterExists(int id)
        {
            return _context.Critter.Any(e => e.CritterId == id);
        }
    }
}
