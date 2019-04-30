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
    [Authorize(Roles = "admin,staff")]
    public class CritterExController : Controller
    {
        readonly LivestockContext _livestock;

        public CritterExController(LivestockContext livestock)
        {
            this._livestock = livestock;
        }

        #region CritterImage
        [ResponseCache(Duration = 60 * 60 * 24 * 7)]
        public async Task<IActionResult> Image(int critterId)
        {
            var critter = await this._livestock.Critter.Include(c => c.CritterImage).FirstAsync(c => c.CritterId == critterId);
            
            if(critter.CritterImageId != null)
            {
                var image = critter.CritterImage.Data;
                return File(image, "image/png");
            }
            else
                return Redirect("https://via.placeholder.com/200");
        }

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
                               .Include(v => v.DadCritter)
                               .Include(v => v.MumCritter)
                               .Include(v => v.OwnerContact)
                               .ToListAsync()
            );
        }

        public async Task<IActionResult> Edit(int? id)
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
            ViewData["BreedId"] = new SelectList(this._livestock.Breed, "BreedId", "Description", val.BreedId);
            ViewData["CritterTypeId"] = new SelectList(this._livestock.CritterType, "CritterTypeId", "Name", val.CritterTypeId);
            ViewData["DadCritterId"] = new SelectList(this._livestock.Critter.Where(c => c.Gender == "M"), "CritterId", "Name", val.DadCritterId);
            ViewData["MumCritterId"] = new SelectList(this._livestock.Critter.Where(c => c.Gender == "F"), "CritterId", "Name", val.MumCritterId);
            ViewData["OwnerContactId"] = new SelectList(this._livestock.Contact, "ContactId", "Name", val.OwnerContactId);
            return View(new CritterExEditViewModel
            {
                Critter = val,
                Javascript = await this._livestock.EnumCritterLifeEventType
                                                  .Where(e => e.DataType != "None")
                                                  .OrderBy(e => e.Description)
                                                  .Select(e => new CritterLifeEventJavascriptInfo{ Name = e.Description, DataType = e.DataType.ToLower() })
                                                  .ToListAsync(),
                LifeEventTableInfo = val.CritterLifeEvent
                                        .Select(e => new CritterLifeEventTableInfo
                                        {
                                             DateTime = e.DateTime,
                                             Description = e.Description,
                                             Type = this._livestock.EnumCritterLifeEventType
                                                                   .FirstAsync(t => t.EnumCritterLifeEventTypeId == e.EnumCritterLifeEventTypeId)
                                                                   .Result
                                                                   .Description,
                                             Id = e.CritterLifeEventId,
                                             DataType = this._livestock.EnumCritterLifeEventType
                                                                       .FirstAsync(t => t.EnumCritterLifeEventTypeId == e.EnumCritterLifeEventTypeId)
                                                                       .Result
                                                                       .DataType

                                        })
                                        .ToList() // For some reason, this can't be async.
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Critter")]CritterExEditViewModel model)
        {
            if (model.Critter.CritterId != id)
                return NotFound();

            if (ModelState.IsValid)
            {
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
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BreedId"] = new SelectList(this._livestock.Breed, "BreedId", "Description", model.Critter.BreedId);
            ViewData["CritterTypeId"] = new SelectList(this._livestock.CritterType, "CritterTypeId", "Name", model.Critter.CritterTypeId);
            ViewData["DadCritterId"] = new SelectList(this._livestock.Critter, "CritterId", "Name", model.Critter.DadCritterId);
            ViewData["MumCritterId"] = new SelectList(this._livestock.Critter, "CritterId", "Name", model.Critter.MumCritterId);
            ViewData["OwnerContactId"] = new SelectList(this._livestock.Contact, "ContactId", "Name", model.Critter.OwnerContactId);
            return View(model);
        }

        public IActionResult Create()
        {
            ViewData["BreedId"] = new SelectList(this._livestock.Breed, "BreedId", "Description");
            ViewData["CritterTypeId"] = new SelectList(this._livestock.CritterType, "CritterTypeId", "Name");
            ViewData["DadCritterId"] = new SelectList(this._livestock.Critter, "CritterId", "Name");
            ViewData["MumCritterId"] = new SelectList(this._livestock.Critter, "CritterId", "Name");
            ViewData["OwnerContactId"] = new SelectList(this._livestock.Contact, "ContactId", "Name");

            return View();
        }

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
        #endregion

        private async Task AddNewEvent(int critterId, int dataId, string eventTypeName, string description)
        {
            var @event = new CritterLifeEvent
            {
                CritterId = critterId,
                Comment = "N/A",
                DateTime = DateTime.Now,
                Description = description,
                EnumCritterLifeEventDataId = dataId,
                EnumCritterLifeEventType = await this._livestock.EnumCritterLifeEventType.FirstAsync(e => e.Description == eventTypeName),
                VersionNumber = 1
            };
            await this._livestock.AddAsync(@event);
            await this._livestock.SaveChangesAsync();
        }

        #region DateTime
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewDateTime(int? id, [Bind("DateTime,EventTypeName,EventDescription")]CritterExEditViewModel model)
        {
            if(id == null)
                throw new Exception("ID isn't being passed.");

            using (var transact = await this._livestock.Database.BeginTransactionAsync())
            {
                var dateTime = new CritterLifeEventDatetime
                {
                    Comment         = "N/A",
                    DateTime        = model.DateTime.DateTime,
                    VersionNumber   = 1
                };
                await this._livestock.AddAsync(dateTime);
                await this._livestock.SaveChangesAsync(); // We have to save here so dateTime gets an Id

                await this.AddNewEvent(id.Value, dateTime.CritterLifeEventGiveBirthId, model.EventTypeName, model.EventDescription);
                transact.Commit();
            }

            return RedirectToAction(nameof(Edit), new { id });
        }
        
        public async Task<IActionResult> EditDateTime(int id)
        {
            var @event = await this._livestock.CritterLifeEvent.FindAsync(id);
            var value = await this._livestock.CritterLifeEventDatetime.FindAsync(@event.EnumCritterLifeEventDataId);
            return View(
                new LifeEventEditDateTime
                {
                    Common = new LifeEventEditCommon
                    {
                        Id = id,
                        Description = @event.Description
                    },

                    DateTime = value.DateTime
                }
            );
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDateTime([Bind("DateTime,Common")]LifeEventEditDateTime model)
        {
            var @event = await this._livestock.CritterLifeEvent.FindAsync(model.Common.Id);
            var value = await this._livestock.CritterLifeEventDatetime.FindAsync(@event.EnumCritterLifeEventDataId);

            @event.Description = model.Common.Description;
            value.DateTime = model.DateTime;

            await this._livestock.SaveChangesAsync();

            return RedirectToAction(nameof(Edit), routeValues: new { id = model.Common.Id });
        }
        
        public async Task<IActionResult> DeleteDateTime(int id)
        {
            // TODO: Make a function like "DateTimeFromId"
            var @event = await this._livestock.CritterLifeEvent.FindAsync(id);
            var value = await this._livestock.CritterLifeEventDatetime.FindAsync(@event.EnumCritterLifeEventDataId);
            return View(
                new LifeEventEditDateTime
                {
                    Common = new LifeEventEditCommon
                    {
                        Id = id,
                        Description = @event.Description
                    },

                    DateTime = value.DateTime
                }
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDateTimePost()
        {
            var id = Convert.ToInt32(Request.Form["Common.Id"]);
            var @event = await this._livestock.CritterLifeEvent.FindAsync(id);
            var value = await this._livestock.CritterLifeEventDatetime.FindAsync(@event.EnumCritterLifeEventDataId);

            this._livestock.CritterLifeEvent.Remove(@event);
            this._livestock.CritterLifeEventDatetime.Remove(value);
            await this._livestock.SaveChangesAsync();

            return RedirectToAction(nameof(Edit), routeValues: new { id });
        }
        #endregion

        private void FixNullFields(Critter val)
        {
            if (String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
            if (String.IsNullOrWhiteSpace(val.DadFurther)) val.DadFurther = "N/A";
            if (String.IsNullOrWhiteSpace(val.Gender)) val.Gender = "?";
            if (String.IsNullOrWhiteSpace(val.MumFurther)) val.MumFurther = "N/A";
            if (String.IsNullOrWhiteSpace(val.Name)) val.Name = "N/A";
        }
    }
}
