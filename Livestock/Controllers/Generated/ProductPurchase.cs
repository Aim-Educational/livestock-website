
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
	public class ProductPurchaseController : Controller
    {
        private readonly LivestockContext _context;

        public ProductPurchaseController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.ProductPurchase.Include(v => v.Product).Include(v => v.Supplier);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.ProductPurchase.Include(v => v.Product).Include(v => v.Supplier).FirstOrDefaultAsync(m => m.ProductPurchaseId == id);
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
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Description");
ViewData["SupplierId"] = new SelectList(_context.Contact, "ContactId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
		[HasPermission(UserPermission.LivestockModify)]
        public async Task<IActionResult> Create([Bind("ProductPurchaseId,BatchNumber,Comment,Cost,DateTime,Expiry,LocationId,ProductId,SupplierId,Timestamp,VersionNumber,Volume")]ProductPurchase val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Description", val.ProductId);
ViewData["SupplierId"] = new SelectList(_context.Contact, "ContactId", "Name", val.SupplierId);
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

            var val = await _context.ProductPurchase.FindAsync(id);
            if (val == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Description", val.ProductId);
ViewData["SupplierId"] = new SelectList(_context.Contact, "ContactId", "Name", val.SupplierId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[AimAuthorize]
		[HasPermission(UserPermission.LivestockModify)]
        public async Task<IActionResult> Edit(int id, [Bind("ProductPurchaseId,BatchNumber,Comment,Cost,DateTime,Expiry,LocationId,ProductId,SupplierId,Timestamp,VersionNumber,Volume")]ProductPurchase val)
        {
			if(val.ProductPurchaseId != id)
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
                    if (!Exists(val.ProductPurchaseId))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Description", val.ProductId);
ViewData["SupplierId"] = new SelectList(_context.Contact, "ContactId", "Name", val.SupplierId);
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

            var val = await _context.ProductPurchase.Include(v => v.Product).Include(v => v.Supplier).FirstOrDefaultAsync(m => m.ProductPurchaseId == id);
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
            var val = await _context.ProductPurchase.FindAsync(id);
            _context.ProductPurchase.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private void FixNullFields(ProductPurchase val)
		{
			if(String.IsNullOrWhiteSpace(val.BatchNumber)) val.BatchNumber = "N/A";
if(String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
		}

        private bool Exists(int id)
        {
            return _context.ProductPurchase.Any(e => e.ProductPurchaseId == id);
        }
    }
}