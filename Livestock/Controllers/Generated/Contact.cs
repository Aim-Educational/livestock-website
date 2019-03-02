
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
	public class ContactController : Controller
    {
        private readonly LivestockContext _context;

        public ContactController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.Contact;
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Contact.FirstOrDefaultAsync(m => m.ContactId == id);
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
        public async Task<IActionResult> Create([Bind("ContactId,Address,Comment,EmailAddress,IsCustomer,IsSupplier,Name,PhoneNumber1,PhoneNumber2,PhoneNumber3,PhoneNumber4,Timestamp,VersionNumber")]Contact val)
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

            var val = await _context.Contact.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
        public async Task<IActionResult> Edit(int id, [Bind("ContactId,Address,Comment,EmailAddress,IsCustomer,IsSupplier,Name,PhoneNumber1,PhoneNumber2,PhoneNumber3,PhoneNumber4,Timestamp,VersionNumber")]Contact val)
        {
			if(val.ContactId != id)
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
                    if (!Exists(val.ContactId))
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

            var val = await _context.Contact.FirstOrDefaultAsync(m => m.ContactId == id);
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
            var val = await _context.Contact.FindAsync(id);
            _context.Contact.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(Contact val)
		{
			if(String.IsNullOrWhiteSpace(val.Address)) val.Address = "N/A";
if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
if(String.IsNullOrWhiteSpace(val.EmailAddress)) val.EmailAddress = "N/A";
if(String.IsNullOrWhiteSpace(val.Name)) val.Name = "N/A";
if(String.IsNullOrWhiteSpace(val.PhoneNumber1)) val.PhoneNumber1 = "N/A";
if(String.IsNullOrWhiteSpace(val.PhoneNumber2)) val.PhoneNumber2 = "N/A";
if(String.IsNullOrWhiteSpace(val.PhoneNumber3)) val.PhoneNumber3 = "N/A";
if(String.IsNullOrWhiteSpace(val.PhoneNumber4)) val.PhoneNumber4 = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.Contact.Any(e => e.ContactId == id);
        }
    }
}