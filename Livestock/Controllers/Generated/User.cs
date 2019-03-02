
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
	public class UserController : Controller
    {
        private readonly LivestockContext _context;

        public UserController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.User.Include(v => v.PreferredMobileNumber);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.User.Include(v => v.PreferredMobileNumber).FirstOrDefaultAsync(m => m.UserId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[AimAuthorize]
        public IActionResult Create()
        {
            ViewData["PreferredMobileNumberId"] = new SelectList(_context.UserMobileNumber, "UserMobileNumberId", "MobileNumber");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Create([Bind("UserId,Comment,Email,Name,Nickname,PreferredMobileNumberId,Timestamp,VersionNumber")]User val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PreferredMobileNumberId"] = new SelectList(_context.UserMobileNumber, "UserMobileNumberId", "MobileNumber", val.PreferredMobileNumberId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.User.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            ViewData["PreferredMobileNumberId"] = new SelectList(_context.UserMobileNumber, "UserMobileNumberId", "MobileNumber", val.PreferredMobileNumberId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Comment,Email,Name,Nickname,PreferredMobileNumberId,Timestamp,VersionNumber")]User val)
        {
			if(val.UserId != id)
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
                    if (!Exists(val.UserId))
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
            ViewData["PreferredMobileNumberId"] = new SelectList(_context.UserMobileNumber, "UserMobileNumberId", "MobileNumber", val.PreferredMobileNumberId);
            return View(val);
        }

		[AimAuthorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.User.Include(v => v.PreferredMobileNumber).FirstOrDefaultAsync(m => m.UserId == id);
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
            var val = await _context.User.FindAsync(id);
            _context.User.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(User val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
if(String.IsNullOrWhiteSpace(val.Email)) val.Email = "N/A";
if(String.IsNullOrWhiteSpace(val.Name)) val.Name = "N/A";
if(String.IsNullOrWhiteSpace(val.Nickname)) val.Nickname = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}