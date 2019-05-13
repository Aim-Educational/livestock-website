using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Models;

namespace Website.Controllers
{
    public class LifeEventController : Controller
    {
        readonly LivestockContext _livestock;

        public LifeEventController(LivestockContext livestock)
        {
            this._livestock = livestock;
        }

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
        [Authorize(Roles = "admin,staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewDateTime(int? id, [Bind("DateTime,EventTypeName,EventDescription")]CritterExEditViewModel model)
        {
            if (id == null)
                throw new Exception("ID isn't being passed.");

            using (var transact = await this._livestock.Database.BeginTransactionAsync())
            {
                var dateTime = new CritterLifeEventDatetime
                {
                    Comment = "N/A",
                    DateTime = model.DateTime.DateTime,
                    VersionNumber = 1
                };
                await this._livestock.AddAsync(dateTime);
                await this._livestock.SaveChangesAsync(); // We have to save here so dateTime gets an Id

                await this.AddNewEvent(id.Value, dateTime.CritterLifeEventGiveBirthId, model.EventTypeName, model.EventDescription);
                transact.Commit();
            }

            return RedirectToAction(nameof(CritterExController.Edit), "CritterEx", new { id });
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

        [Authorize(Roles = "admin,staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDateTime([Bind("DateTime,Common")]LifeEventEditDateTime model)
        {
            var @event = await this._livestock.CritterLifeEvent.FindAsync(model.Common.Id);
            var value = await this._livestock.CritterLifeEventDatetime.FindAsync(@event.EnumCritterLifeEventDataId);

            @event.Description = model.Common.Description;
            value.DateTime = model.DateTime;

            await this._livestock.SaveChangesAsync();

            return RedirectToAction(nameof(CritterExController.Index), "CritterEx");
        }

        [Authorize(Roles = "admin,staff")]
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

        [Authorize(Roles = "admin,staff")]
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

            return RedirectToAction(nameof(CritterExController.Index), "CritterEx");
        }
        #endregion
    }
}
