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

        #region Common operations
        private async Task AddNewEvent(int critterId, int eventId, string eventTypeName, string description)
        {
            var eventType = await this._livestock.EnumCritterLifeEventType.FirstAsync(e => e.Description == eventTypeName);
            if(!eventType.AllowMultiple)
            {
                var critter = await this._livestock.Critter.Include(c => c.CritterLifeEvent).FirstAsync(c => c.CritterId == critterId);
                if(critter.CritterLifeEvent.Any(e => e.EnumCritterLifeEventTypeId == eventType.EnumCritterLifeEventTypeId))
                    throw new OnlyOneEventAllowedException($"Only one '{eventTypeName}' event is allowed per critter.");
            }

            var @event = new CritterLifeEvent
            {
                CritterId = critterId,
                Comment = "N/A",
                DateTime = DateTime.Now,
                Description = description,
                EnumCritterLifeEventDataId = eventId,
                EnumCritterLifeEventType = eventType,
                VersionNumber = 1
            };
            await this._livestock.AddAsync(@event);

            // TODO: Handle other flags.
            if(eventType.FlagCantReproduce)
            {
                var critter = await this._livestock.Critter.FindAsync(critterId);
                critter.UpdateFlag(CritterFlags.ReproduceNoLifeEvent, true);
            }

            await this._livestock.SaveChangesAsync();
        }
        #endregion

        #region DateTime(POST)
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

                try
                {
                    await this.AddNewEvent(id.Value, dateTime.CritterLifeEventGiveBirthId, model.EventTypeName, model.EventDescription);
                    transact.Commit();
                }
                catch(OnlyOneEventAllowedException)
                {
                    transact.Rollback();
                }
            }

            return RedirectToAction(nameof(CritterExController.Edit), "CritterEx", new { id });
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

        #region DateTime(GET)
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
        public async Task<IActionResult> DeleteDateTime(int id)
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
        #endregion
    }


    [Serializable]
    public class OnlyOneEventAllowedException : Exception
    {
        public OnlyOneEventAllowedException() { }
        public OnlyOneEventAllowedException(string message) : base(message) { }
        public OnlyOneEventAllowedException(string message, Exception inner) : base(message, inner) { }
        protected OnlyOneEventAllowedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
