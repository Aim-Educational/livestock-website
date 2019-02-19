
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
	public class ProductController : Controller
    {
        private readonly LivestockContext _context;

        public ProductController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.Product.Include(v => v.EnumProductType);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.Product.Include(v => v.EnumProductType).FirstOrDefaultAsync(m => m.ProductId == id);
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
            ViewData["EnumProductTypeId"] = new SelectList(_context.EnumProductType, "EnumProductTypeId", "Description");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
		[HasPermission(UserPermission.LivestockModify)]
        public async Task<IActionResult> Create([Bind("ProductId,Comment,DefaultVolume,Description,EnumProductTypeId,RequiresRefridgeration,Timestamp,VersionNumber")]Product val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnumProductTypeId"] = new SelectList(_context.EnumProductType, "EnumProductTypeId", "Description", val.EnumProductTypeId);
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

            var val = await _context.Product.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            ViewData["EnumProductTypeId"] = new SelectList(_context.EnumProductType, "EnumProductTypeId", "Description", val.EnumProductTypeId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
		[HasPermission(UserPermission.LivestockModify)]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Comment,DefaultVolume,Description,EnumProductTypeId,RequiresRefridgeration,Timestamp,VersionNumber")]Product val)
        {
			if(val.ProductId != id)
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
                    if (!Exists(val.ProductId))
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
            ViewData["EnumProductTypeId"] = new SelectList(_context.EnumProductType, "EnumProductTypeId", "Description", val.EnumProductTypeId);
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

            var val = await _context.Product.Include(v => v.EnumProductType).FirstOrDefaultAsync(m => m.ProductId == id);
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
            var val = await _context.Product.FindAsync(id);
            _context.Product.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(Product val)
		{
			if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
if(String.IsNullOrWhiteSpace(val.Description)) val.Description = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}