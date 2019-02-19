
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
	public class EnumProductTypeController : Controller
    {
        private readonly LivestockContext _context;

        public EnumProductTypeController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.EnumProductType;
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.EnumProductType.FirstOrDefaultAsync(m => m.EnumProductTypeId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[AimAuthorize]
		[HasPermission(UserPermission.LivestockModify)]
        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
		[HasPermission(UserPermission.LivestockModify)]
        public async Task<IActionResult> Create([Bind("EnumProductTypeId,Comment,Description,Timestamp,VersionNumber")]EnumProductType val)
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
		[HasPermission(UserPermission.LivestockModify)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.EnumProductType.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
		[HasPermission(UserPermission.LivestockModify)]
        public async Task<IActionResult> Edit(int id, [Bind("EnumProductTypeId,Comment,Description,Timestamp,VersionNumber")]EnumProductType val)
        {
			if(val.EnumProductTypeId != id)
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
                    if (!Exists(val.EnumProductTypeId))
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
		[HasPermission(UserPermission.LivestockModify)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.EnumProductType.FirstOrDefaultAsync(m => m.EnumProductTypeId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
		[HasPermission(UserPermission.LivestockModify)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var val = await _context.EnumProductType.FindAsync(id);
            _context.EnumProductType.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(EnumProductType val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
if(String.IsNullOrWhiteSpace(val.Description)) val.Description = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.EnumProductType.Any(e => e.EnumProductTypeId == id);
        }
    }
}