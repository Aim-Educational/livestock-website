
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
	public class AlUserInfoController : Controller
    {
        private readonly LivestockContext _context;

        public AlUserInfoController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.AlUserInfo;
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.AlUserInfo.FirstOrDefaultAsync(m => m.AlUserInfoId == id);
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
        public async Task<IActionResult> Create([Bind("AlUserInfoId,Comment,EmailAddress,FirstName,LastName,PrivacyConsent,Timestamp,TosConsent")]AlUserInfo val)
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

            var val = await _context.AlUserInfo.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("AlUserInfoId,Comment,EmailAddress,FirstName,LastName,PrivacyConsent,Timestamp,TosConsent")]AlUserInfo val)
        {
			if(val.AlUserInfoId != id)
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
                    if (!Exists(val.AlUserInfoId))
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

            var val = await _context.AlUserInfo.FirstOrDefaultAsync(m => m.AlUserInfoId == id);
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
            var val = await _context.AlUserInfo.FindAsync(id);
            _context.AlUserInfo.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(AlUserInfo val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
if(String.IsNullOrWhiteSpace(val.FirstName)) val.FirstName = "N/A";
if(String.IsNullOrWhiteSpace(val.LastName)) val.LastName = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.AlUserInfo.Any(e => e.AlUserInfoId == id);
        }
    }
}