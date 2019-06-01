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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Buffers;
using System.Runtime;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Microsoft.AspNetCore.Hosting;

namespace Website.Controllers
{
    [Authorize(Roles = "admin,staff,student")]
    public class CritterExController : Controller
    {
        const string IMAGE_CACHE_FOLDER = "ImageCache";

        static readonly ArrayPool<byte> _imageBufferPool = ArrayPool<byte>.Create(1 * 1024 * 1024, 20);

        readonly LivestockContext _livestock;
        readonly IHostingEnvironment _environment;

        public CritterExController(LivestockContext livestock, IHostingEnvironment environment)
        {
            this._livestock = livestock;
            this._environment = environment;
        }

        #region CritterImage
        [ResponseCache(Duration = 60 * 60 * 24 * 365)]
        public async Task<IActionResult> Image(int critterId, int cacheVersion, int? width, int? height)
        {
            var critter = await this._livestock.Critter.FirstAsync(c => c.CritterId == critterId);
            
            // If the critter has an image, retrieve it.
            if(critter.CritterImageId != null)
            {
                CritterImage image = null;

                // If we need a specific size, either retrieve or create it.
                if(width != null && height != null)
                {
                    var variant = await this._livestock.CritterImageVariant
                                                       .FirstOrDefaultAsync(v => v.CritterImageOriginalId == critter.CritterImageId
                                                                              && v.Width == width
                                                                              && v.Height == height);

                    if(variant != null)
                    {
                        return await this.GetAndCacheImage(critter, variant.CritterImageModifiedId, cacheVersion, width, height);
                    }
                    else
                    {
                        variant = new CritterImageVariant
                        {
                            CritterImageOriginalId = critter.CritterImageId.Value,
                            Width = width.Value,
                            Height = height.Value
                        };

                        await this._livestock.Entry(critter).Reference(c => c.CritterImage).LoadAsync();
                        image = critter.CritterImage;

                        // Resize it, then upload it so it's cached.
                        var resized = await this.ResizeImageAsyncPOOLED(image.Data, width.Value, height.Value);
                        image = new CritterImage
                        {
                            Data = resized
                        };

                        variant.CritterImageModified = image;

                        try
                        {
                            await this._livestock.CritterImage.AddAsync(image);
                            await this._livestock.CritterImageVariant.AddAsync(variant);
                            await this._livestock.SaveChangesAsync();
                        }
                        finally // I can count on one hand the amount of times I've used 'finally'.
                        {
                            // We detach the entity, since the image data shouldn't be reference anymore once it's returned.
                            // And if we set it to 'null', then we can accidentally remove the image data if we save at some point.
                            // So it's safer to just detach it, and take the performance hit of downloading a new version of it when needed.
                            this._livestock.Entry(image).State = EntityState.Detached;
                            _imageBufferPool.Return(resized);
                        }
                    }
                }
                else // Otherwise, return the original.
                {
                    await this._livestock.Entry(critter).Reference(c => c.CritterImage).LoadAsync();
                    image = critter.CritterImage;
                }
                
                // Hopefully force the GC to clean up the LOH
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;

                return File(image.Data, "image/png");
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
                                                           .ThenInclude(i => i.CritterImageVariantCritterImageOriginal)
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

                // Remove all variants of this image from the database.
                var variants = critter.CritterImage.CritterImageVariantCritterImageOriginal.ToList();
                foreach(var variant in variants)
                {
                    this._livestock.CritterImage.Remove(await this._livestock.CritterImage.FindAsync(variant.CritterImageModifiedId));
                    this._livestock.Remove(variant);
                }

                // Then resize the image to a standard resolution we need.
                // Keep in mind that my iphone takes them in 4k. We're never gonna need them in 4k for anything.
                var resized = await this.ResizeImageAsyncPOOLED(critter.CritterImage.Data, 1920, 1080, "png");

                try
                {
                    critter.CritterImage.Data = resized;
                    await this._livestock.SaveChangesAsync();
                }
                finally
                {
                    this._livestock.Entry(critter).State = EntityState.Detached;
                    _imageBufferPool.Return(resized);
                }
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
                               .OrderBy(v => v.Name)
                               .ToListAsync()
            );
        }

        public async Task<IActionResult> Delete(int id)
        {
            var val = await this._livestock.Critter
                                .Include(c => c.CritterLifeEvent)
                                .Include(c => c.InverseDadCritter)
                                .Include(c => c.InverseMumCritter)
                                .FirstAsync(c => c.CritterId == id);

            if(val == null)
                return NotFound();

            string cantDeleteReason;
            var canDelete = val.CanSafelyDelete(out cantDeleteReason);

            if(!canDelete)
                return RedirectToAction("Error", "Home", new { message = cantDeleteReason });

            this._livestock.Critter.Remove(val);
            await this._livestock.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return NotFound();
            }

            var val = await this._livestock.Critter
                                           .Include(c => c.CritterLifeEvent)
                                           .Include(c => c.InverseDadCritter)
                                           .Include(c => c.InverseMumCritter)
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
                model.Critter.UpdateFlag(CritterFlags.ReproduceYesUser, model.YesReproduceUser);

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
        public async Task<IActionResult> Create([Bind("Critter,File,YesReproduceUser")] CritterExCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                this.FixNullFields(model.Critter);
                model.Critter.UpdateFlag(CritterFlags.ReproduceYesUser, model.YesReproduceUser);

                await this._livestock.AddAsync(model.Critter);
                await this._livestock.SaveChangesAsync();
                
                return (model.File == null) ? RedirectToAction("Index")
                                            : await this.Image(model.File, model.Critter.CritterId);
            }

            return View(model);
        }

        private void SetupCritterViewData(Critter val)
        {
            ViewData["CritterTypeId"]  = new SelectList(this._livestock.CritterType.OrderBy(c => c.Name),                                                "CritterTypeId", "Name", val?.CritterTypeId);
            ViewData["DadCritterId"]   = new SelectList(this._livestock.Critter.Where(c => c.Gender == "M" || c.Name == "Unknown").OrderBy(c => c.Name), "CritterId",     "Name", val?.DadCritterId ?? -1);
            ViewData["MumCritterId"]   = new SelectList(this._livestock.Critter.Where(c => c.Gender == "F" || c.Name == "Unknown").OrderBy(c => c.Name), "CritterId",     "Name", val?.MumCritterId ?? -1);
            ViewData["OwnerContactId"] = new SelectList(this._livestock.Contact.OrderBy(c => c.Name),                                                    "ContactId",     "Name", val?.OwnerContactId);
        }
        #endregion

        #region Critter AJAX
        private IQueryable<Critter> GetCrittersFilteredNoRender(CritterExGetCrittersFilteredAjax ajax)
        {
            return this._livestock.Critter
                                  .Include(v => v.Breed)
                                  .Include(v => v.CritterType)
                                  .Include(v => v.DadCritter)
                                  .Include(v => v.MumCritter)
                                  .Include(v => v.OwnerContact)
                                  .Where(c => (ajax.BreedId ?? -999) == -999
                                           || c.BreedId == ajax.BreedId)
                                  .Where(c => (ajax.CritterTypeId ?? -999) == -999
                                           || c.CritterTypeId == ajax.CritterTypeId)
                                  .Where(c => ajax.Gender == null
                                           || c.Gender == ajax.Gender)
                                  .Where(c => ajax.CanReproduce == null
                                           || c.CanReproduce == ajax.CanReproduce)
                                  .Where(c => String.IsNullOrWhiteSpace(ajax.Name)
                                           || Regex.IsMatch(c.Name.ToLower(), ajax.Name.ToLower()))
                                  .Where(c => String.IsNullOrWhiteSpace(ajax.Tag)
                                           || Regex.IsMatch(c.TagNumber.ToLower(), ajax.Tag.ToLower()))
                                  .OrderBy(c => c.Name);
        }

        // The way this is supposed to work is:
        //  - User messes with the filters in some way.
        //  - Filter sets off an AJAX request.
        //  - This handler will perform the filtering, then render the design the request wants.
        //  - This is then returned, and the rendered HTML is directly embedded by the AJAX requesting code.
        [HttpPost]
        public async Task<IActionResult> GetCrittersFilteredAndRender([FromBody] CritterExGetCrittersFilteredAjax ajax)
        {
            var list = await this.GetCrittersFilteredNoRender(ajax).ToListAsync();

            if(ajax.Design == "card-horiz")
                return PartialView("_CardHoriz", list);
            else if(ajax.Design == "card-vert")
                return PartialView("_CardVert", list);
            else if(ajax.Design == "table")
                return PartialView("_Table", list);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> GetCrittersFilteredValueDescription([FromBody] CritterExGetCrittersFilteredAjax ajax)
        {
            return Json(await this.GetCrittersFilteredNoRender(ajax)
                                  .Select(c => new { value = c.CritterId, description = c.Name })
                                  .ToListAsync()
            );
        }

        // Returns a JSON array containing a Description-Value pair.
        // Description is what can be displayed to the user.
        // Value is the ID of the breed being described.
        [HttpPost]
        public async Task<IActionResult> GetBreedList([FromBody] CritterExGetBreedListAjax ajax)
        {
            if (ajax == null)
                return Json(null);

            var breeds = await this._livestock.Breed
                                              .Include(b => b.Critter)
                                              .Where(b => b.CritterTypeId == ajax.CritterTypeId || b.Description == "Unknown")
                                              // Line below: "[Type Description] ([Count of critter for type])"
                                              // Large mess of code = If type is unknown, only count the critters of the wanted type, instead of every type.
                                              .Select(b => new { description = $"{b.Description} ({((b.Description == "Unknown") ? b.Critter.Where(c => c.CritterTypeId == ajax.CritterTypeId).Count() : b.Critter.Count)})", value = b.BreedId })
                                              .OrderBy(b => b.description)
                                              .ToListAsync();
            return Json(breeds);
        }
        #endregion

        #region Utility
        private async Task<IActionResult> GetAndCacheImage(Critter critter, int critterImageId, int cacheVersion, int? width, int? height)
        {
            var cachePath = Path.Combine(this._environment.WebRootPath, IMAGE_CACHE_FOLDER, $"{critter.CritterId}-{width ?? 0}-{height ?? 0}-{cacheVersion}.png");
            if(!System.IO.File.Exists(cachePath))
            {
                if(!Directory.Exists(Path.GetDirectoryName(cachePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(cachePath));

                // Get the image data, and cache it to the file system (faster downloading for the user).
                // We're manually executing the request, since EF butchers memory usage in this case.
                using (var command = this._livestock.Database.GetDbConnection().CreateCommand())
                {
                    var imageId = command.CreateParameter();
                    imageId.ParameterName = "@ImageId";
                    imageId.Value = critterImageId;
                    command.Parameters.Add(imageId);

                    command.CommandText = "SELECT data FROM dbo.critter_image WHERE critter_image_id = @ImageId;";
                    command.Connection.Open();
                    using (var dbStream = await command.ExecuteReaderAsync(System.Data.CommandBehavior.SequentialAccess | System.Data.CommandBehavior.SingleResult))
                    {
                        using (var fileStream = System.IO.File.Create(cachePath))
                        {
                            while (await dbStream.ReadAsync())
                            {
                                var stream = dbStream.GetStream(0);
                                await stream.CopyToAsync(fileStream);
                            }
                        }
                    }
                }
            }

            return PhysicalFile(cachePath, "image/png");
        }

        private void FixNullFields(Critter val)
        {
            if (String.IsNullOrWhiteSpace(val.Comment)) val.Comment = "N/A";
            if (String.IsNullOrWhiteSpace(val.DadFurther)) val.DadFurther = "N/A";
            if (String.IsNullOrWhiteSpace(val.Gender)) val.Gender = "?";
            if (String.IsNullOrWhiteSpace(val.MumFurther)) val.MumFurther = "N/A";
            if (String.IsNullOrWhiteSpace(val.Name)) val.Name = "N/A";
        }

        // "POOLED" is to make it very clear that the memory returned is from the image pool.
        private Task<byte[]> ResizeImageAsyncPOOLED(byte[] data, int width, int height, string format = "jpg")
        {
            // Ran in another thread since this is a very expensive operation on our underpowered droplet.
            // Especially so, since my iphone takes images in 4k...
            return Task.Run<byte[]>(() => 
            {
                using (var toEdit = SixLabors.ImageSharp.Image.Load(data))
                {
                    toEdit.Mutate(i => i.Resize(width, height));

                    using (var memory = new MemoryStream())
                    {
                        if(format == "jpg")
                            toEdit.SaveAsJpeg(memory);
                        else if(format == "png")
                            toEdit.SaveAsPng(memory, new SixLabors.ImageSharp.Formats.Png.PngEncoder{ CompressionLevel = 9 });
                        else
                            throw new InvalidOperationException($"No format for '{format}'");

                        memory.Position = 0;                    
                        var toReturn = _imageBufferPool.Rent((int)memory.Length);
                        memory.ReadAsync(toReturn);

                        return toReturn;
                    }
                }
            });
        }
        #endregion
    }

    public class CritterExGetBreedListAjax
    {
        public int CritterTypeId { get; set; }
    }

    // Any combination of filters is supported.
    // Everything can be null.
    public class CritterExGetCrittersFilteredAjax
    {
        public int? BreedId { get; set; }
        public int? CritterTypeId { get; set; }
        public string Design { get; set; }
        public string Gender { get; set; }
        public bool? CanReproduce { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
    }
}
