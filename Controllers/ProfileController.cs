using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProfileController:Controller
    {
        /**public IActionResult Index()
        {
            return View();
        }**/
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;


        public ProfileController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Photos/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<ImageMetadata> metadata, IEnumerable<IFormFile> imageFiles)
        {
            // Iterate through the list of image files
            for (int i = 0; i < imageFiles.Count(); i++)
            {
                var image = imageFiles.ElementAt(i);
                if (image.Length > 0)
                {
                    // Create a new Photos object for each file
                    var photos = new Photos
                    {
                        Category = metadata[i].Category,
                        Description = metadata[i].Description
                    };

                    // Save image to wwwroot/image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = image.FileName;
                    photos.ImagePath = "~/Image/" + fileName;
                    string path = Path.Combine(wwwRootPath + "/Image/", image.FileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    // Generate thumbnail and save to wwwroot/image/thumbnail
                    string thumbnailPath = Path.Combine(wwwRootPath + "/Image/Thumbnail/", image.FileName);
                    using (var imageStream = new FileStream(path, FileMode.Open))
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

            return RedirectToAction("Index", "Home");
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

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        private bool PhotosExists(int id)
        {
            return _context.Photos.Any(e => e.Id == id);
        }
    }
}

