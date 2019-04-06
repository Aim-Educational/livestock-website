
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
	public class ProductPurchaseController : Controller
    {
        private readonly LivestockContext _context;

        public ProductPurchaseController(LivestockContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livestockContext = _context.ProductPurchase.Include(v => v.Location).Include(v => v.Product).Include(v => v.Supplier);
            return View(await livestockContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.ProductPurchase.Include(v => v.Location).Include(v => v.Product).Include(v => v.Supplier).FirstOrDefaultAsync(m => m.ProductPurchaseId == id);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

		[Authorize]
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "Name");
ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Description");
ViewData["SupplierId"] = new SelectList(_context.Contact, "ContactId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public async Task<IActionResult> Create([Bind("ProductPurchaseId,BatchNumber,Comment,Cost,DateTime,Expiry,LocationId,ProductId,SupplierId,Timestamp,VersionNumber,Volume")]ProductPurchase val)
        {
			this.FixNullFields(val);
            if (ModelState.IsValid)
            {
                _context.Add(val);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "Name", val.LocationId);
ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Description", val.ProductId);
ViewData["SupplierId"] = new SelectList(_context.Contact, "ContactId", "Name", val.SupplierId);
            return View(val);
        }

		[Authorize]
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
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "Name", val.LocationId);
ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Description", val.ProductId);
ViewData["SupplierId"] = new SelectList(_context.Contact, "ContactId", "Name", val.SupplierId);
            return View(val);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
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
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "Name", val.LocationId);
ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Description", val.ProductId);
ViewData["SupplierId"] = new SelectList(_context.Contact, "ContactId", "Name", val.SupplierId);
            return View(val);
        }

		[Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await _context.ProductPurchase.Include(v => v.Location).Include(v => v.Product).Include(v => v.Supplier).FirstOrDefaultAsync(m => m.ProductPurchaseId == id);
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