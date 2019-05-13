using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AimLogin.DbModel;
using AimLogin.Services;
using Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Website.Models;
using Website.Other;

namespace Website.Controllers
{
    [Authorize(Roles = "admin,staff,student")]
    public class CritterExController : Controller
    {
        readonly LivestockContext _livestock;

        public CritterExController(LivestockContext livestock)
        {
            this._livestock = livestock;
        }

        #region CritterImage
        [ResponseCache(Duration = 60 * 60 * 24 * 365)]
        public async Task<IActionResult> Image(int critterId, int cacheVersion) // cacheVersion is unused, but needs to be there for routing.
        {
            var critter = await this._livestock.Critter.Include(c => c.CritterImage).FirstAsync(c => c.CritterId == critterId);
            
            if(critter.CritterImageId != null)
            {
                var image = critter.CritterImage.Data;
                return File(image, "image/png");
            }
            else
                return Redirect("/images/icons/default.png");
        }
        
        [Authorize(Roles = "admin,staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Image(IFormFile file, [Bind("critterId")] int critterId)
        {
            if(ModelState.IsValid)
            {
                if(!file.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("file", "The file is not an image.");
                    return RedirectToAction("Edit", new { id = critterId });
                }

                var critter = await this._livestock.Critter.Include(c => c.CritterImage)
                                                           .FirstAsync(c => c.CritterId == critterId);
                
                if(critter.CritterImage == null)
                {
                    critter.CritterImage = new CritterImage();
                    this._livestock.Add(critter.CritterImage);
                }

                var stream = new MemoryStream();

                await file.CopyToAsync(stream);
                stream.Position = 0;

                critter.CritterImage.Data = new byte[stream.Length];
                stream.Read(critter.CritterImage.Data, 0, (int)stream.Length);

                // Basically, we embed the version number of the critter in the image request URL.
                // What this means is, browsers can cache the URL for version 1, but when version 2 is made, they need to download
                // and cache the version 2 image of the critter.
                // This allows us to cache, while still being able to update the images in a timely manner.
                critter.VersionNumber++;
                await this._livestock.SaveChangesAsync();
            }

            return RedirectToAction("Edit", new { id = critterId });
        }
        #endregion

        #region Critter
        public async Task<IActionResult> Index()
        {
            return View(
                await
                this._livestock.Critter
                               .Include(v => v.Breed)
                               .Include(v => v.CritterType)
                                .ThenInclude(t => t.Critter)
                               .Include(v => v.DadCritter)
                               .Include(v => v.MumCritter)
                               .Include(v => v.OwnerContact)
                               .ToListAsync()
            );
        }

        public async Task<IActionResult> Edit(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await this._livestock.Critter
                                           .Include(c => c.CritterLifeEvent)
                                           .FirstAsync(c => c.CritterId == id);
            if (val == null)
            {
                return NotFound();
            }
            this.SetupCritterViewData(val);
            return View(new CritterExEditViewModel
            {
                Critter = val,
                ConcurrencyError = concurrencyError ?? false,
                Javascript = await this._livestock.EnumCritterLifeEventType
                                                  .Where(e => e.DataType != "None")
                                                  .OrderBy(e => e.Description)
                                                  .Select(e => new CritterLifeEventJavascriptInfo{ Name = e.Description, DataType = e.DataType.ToLower() })
                                                  .ToListAsync(),
                LifeEventTableInfo = val.CritterLifeEvent
                                        .Select(e =>
                                        {
                                            var type = this._livestock.EnumCritterLifeEventType
                                                                      .FirstAsync(t => t.EnumCritterLifeEventTypeId == e.EnumCritterLifeEventTypeId)
                                                                      .Result;
                                            return new CritterLifeEventTableInfo
                                            {
                                                 DateTime = e.DateTime,
                                                 Description = e.Description,
                                                 Type = type.Description,
                                                 Id = e.CritterLifeEventId,
                                                 DataType = type.DataType
                                            };
                                        })
                                        .ToList() // For some reason, this can't be async.
            });
        }

        [Authorize(Roles = "admin,staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Critter,YesReproduceUser")]CritterExEditViewModel model)
        {
            if (model.Critter.CritterId != id)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Update reproduce flags
                model.Critter.UpdateReproduceFlag(ReproduceFlags.YesReproduceUser, model.YesReproduceUser);

                // Update Database model
                try
                {
                    this._livestock.Update(model.Critter);
                    await this._livestock.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this._livestock.Critter.Any(c => c.CritterId == model.Critter.CritterId))
                        return NotFound();
                    else
                        return RedirectToAction("Edit", "CritterEx", new { id, concurrencyError = true });
                }
                catch
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }
            this.SetupCritterViewData(model.Critter);
            return View(model);
        }

        public IActionResult Create()
        {
            this.SetupCritterViewData(null);

            return View();
        }

        [Authorize(Roles = "admin,staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Critter,File")] CritterExCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                this.FixNullFields(model.Critter);

                await this._livestock.AddAsync(model.Critter);
                await this._livestock.SaveChangesAsync();

                return await this.Image(model.File, model.Critter.CritterId);
            }

            return View(model);
        }

        private void SetupCritterViewData(Critter val)
        {
            ViewData["CritterTypeId"]  = new SelectList(this._livestock.CritterType.OrderBy(c => c.Name),                         "CritterTypeId", "Name", val?.CritterTypeId);
            ViewData["DadCritterId"]   = new SelectList(this._livestock.Critter.Where(c => c.Gender == "M").OrderBy(c => c.Name), "CritterId",     "Name", val?.DadCritterId);
            ViewData["MumCritterId"]   = new SelectList(this._livestock.Critter.Where(c => c.Gender == "F").OrderBy(c => c.Name), "CritterId",     "Name", val?.MumCritterId);
            ViewData["OwnerContactId"] = new SelectList(this._livestock.Contact.OrderBy(c => c.Name),                             "ContactId",     "Name", val?.OwnerContactId);
        }
        #endregion

        #region Critter AJAX
        [HttpPost] // This is a POST since it's an AJAX request.
        public async Task<IActionResult> GetCrittersFiltered([FromBody] CritterExGetCrittersFilteredAjax ajax)
        {
            var list = await this._livestock.Critter
                                 .Include(v => v.Breed)
                                 .Include(v => v.CritterType)
                                 .Include(v => v.DadCritter)
                                 .Include(v => v.MumCritter)
                                 .Include(v => v.OwnerContact)
                                 .Where(c => ajax.BreedId == -999
                                          || c.BreedId == ajax.BreedId)
                                 .Where(c => ajax.CritterTypeId == -999
                                          || c.CritterTypeId == ajax.CritterTypeId)
                                 .Where(c => ajax.Gender == null
                                          || c.Gender == ajax.Gender)
                                 .Where(c => ajax.CanReproduce == null
                                          || c.CanReproduce == ajax.CanReproduce)
                                 .ToListAsync();

            if(ajax.Design == "card-horiz")
                return PartialView("_CardHoriz", list);
            else if(ajax.Design == "card-vert")
                return PartialView("_CardVert", list);
            else if(ajax.Design == "table")
                return PartialView("_Table", list);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> GetBreedList([FromBody] CritterExGetBreedListAjax ajax)
        {
            if (ajax == null)
                return Json(null);

            var breeds = await this._livestock.Breed
                                              .Include(b => b.Critter)
                                              .Where(b => b.CritterTypeId == ajax.CritterTypeId || b.Description == "Unknown")
                                              .Select(b => new { description = $"{b.Description} ({b.Critter.Count})", value = b.BreedId })
                                              .OrderBy(b => b.description)
                                              .ToListAsync();
            return Json(breeds);
        }
        #endregion

        #region Utility
        private void FixNullFields(Critter val)
        {
            if (String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
            if (String.IsNullOrWhiteSpace(val.DadFurther)) val.DadFurther = "N/A";
            if (String.IsNullOrWhiteSpace(val.Gender)) val.Gender = "?";
            if (String.IsNullOrWhiteSpace(val.MumFurther)) val.MumFurther = "N/A";
            if (String.IsNullOrWhiteSpace(val.Name)) val.Name = "N/A";
        }
        #endregion
    }

    public class CritterExGetBreedListAjax
    {
        public int CritterTypeId { get; set; }
    }

    public class CritterExGetCrittersFilteredAjax
    {
        public int? BreedId { get; set; }
        public int? CritterTypeId { get; set; }
        public string Design { get; set; }
        public string Gender { get; set; }
        public bool? CanReproduce { get; set; }
    }
}
