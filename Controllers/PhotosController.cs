using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using WebApplication1.Data;
using WebApplication1.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


namespace WebApplication1.Controllers
{
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;


        public PhotosController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Photos
        public async Task<IActionResult> Index()
        {
              return View(await _context.Photos.ToListAsync());
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Photos == null)
            {
                return NotFound();
            }

            var photos = await _context.Photos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photos == null)
            {
                return NotFound();
            }

            return View(photos);
        }

        // GET: Photos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Category, string Description, IEnumerable<IFormFile> ImageFiles)
        {
            // Iterate through the list of image files

            foreach (var imageFile in ImageFiles)
            {
                if (imageFile.Length > 0)
                {
                    // Create a new Photos object for each file
                    var photos = new Photos
                    {
                        Category = Category,
                        Description = Description
                    };

                    // Save image to wwwroot/image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = imageFile.FileName;
                    string imagePath = Path.Combine(wwwRootPath + "/Image/", imageFile.FileName);
                    photos.ImagePath = imagePath;
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    // Generate thumbnail and save to wwwroot/image/thumbnail
                    string thumbnailPath = Path.Combine(wwwRootPath + "/Image/Thumbnail/", imageFile.FileName);
                    using (var imageStream = new FileStream(imagePath, FileMode.Open))
                    using (var thumbnailStream = new FileStream(thumbnailPath, FileMode.Create))
                    {
                        var imageObject = SixLabors.ImageSharp.Image.Load(imageStream);
                        imageObject.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new SixLabors.ImageSharp.Size(0, 200),
                            Mode = ResizeMode.Max
                        }));
                        imageObject.SaveAsJpeg(thumbnailStream);
                    }

                    // Insert record
                    _context.Add(photos);
                    await _context.SaveChangesAsync();
                }
            }


            return RedirectToAction(nameof(Index));
        }


        // GET: Photos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Photos == null)
            {
                return NotFound();
            }

            var photos = await _context.Photos.FindAsync(id);
            if (photos == null)
            {
                return NotFound();
            }
            return View(photos);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Category,ImagePath,Description,DateAdded")] Photos photos)
        {
            if (id != photos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotosExists(photos.Id))
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
            return View(photos);
        }

        // GET: Photos/Delete/5g
        public async Task<IActionResult> Delete(int? id)
        {
            int g;
            if (id == null || _context.Photos == null)
            {
                return NotFound();
            }

            var photos = await _context.Photos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photos == null)
            {
                return NotFound();
            }

            return View(photos);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imageModel = await _context.Photos.FindAsync(id);

            //delete image from wwwroot/image
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", imageModel.ImagePath);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
            //delete the record
            _context.Photos.Remove(imageModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }


        private bool PhotosExists(int id)
        {
          return _context.Photos.Any(e => e.Id == id);
        }
    }
}
