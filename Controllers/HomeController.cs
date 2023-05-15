using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.DependencyResolver;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using WebApplication1.Data;

using WebApplication1.Models;




namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private static int currentId;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
           List<Photos> photo = _context.Photos
                 .FromSqlRaw($"SELECT * FROM Photos order by DateAdded desc;")
             .ToList();
            return View(photo.Take(5));

        }
        public IActionResult Privacy()
        {
            return View();
        }
        ViewModel vm = new ViewModel();
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
            
            vm.Comments = _context.Comments
                .FromSqlRaw($"SELECT * FROM Comments WHERE IdPhotos = {id}", id)
                .ToList();
            vm.photo= _context.Photos
                .FromSqlRaw($"SELECT * FROM Photos WHERE Id = {id}", id)
            .ToList()[0];

            currentId = vm.photo.Id;
          
            return View(vm);
        }


        [HttpPost]
        public ActionResult Details(ViewModel model)
        {
           

            Comments comment = new Comments();

            String newCommString = model.newComment;

            if(newCommString != null)
            {
                comment.Text = newCommString;
                comment.IdPhotos =currentId;

                _context.Comments.Add(comment);

                _context.SaveChanges();
                
            }

            vm.photo = _context.Photos
                        .FromSqlRaw($"SELECT * FROM Photos WHERE Id = {currentId}",currentId)
                        .ToList()[0];
            vm.Comments = _context.Comments
                        .FromSqlRaw($"SELECT * FROM Comments WHERE IdPhotos = {currentId}", currentId)
                        .ToList();

            return View(vm);

        }

        public ActionResult Search(string searchString)
        {
            if (_context.Photos == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            var photos = from m in _context.Photos
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                photos = photos.Where(s => s.Category!.Contains(searchString) || s.Description!.Contains(searchString));
                System.Diagnostics.Debug.Write(Newtonsoft.Json.JsonConvert.SerializeObject(photos));
                return View(photos.ToList());
            }
  
            return View("SearchResult");
        }


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

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imageModel = await _context.Photos.FindAsync(id);

            //delete image from wwwroot/image
            // var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", imageModel.ImagePath);
            if (System.IO.File.Exists(imageModel.ImagePath))
                System.IO.File.Delete(imageModel.ImagePath);
            //delete the record
            _context.Photos.Remove(imageModel);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        public  ActionResult DeleteComment(int idComment)
        {
            var comment = _context.Comments.Find(idComment);
            if (comment != null) { 
            _context.Comments.Remove(comment);
                _context.SaveChanges();

                vm.photo = _context.Photos
                       .FromSqlRaw($"SELECT * FROM Photos WHERE Id = {currentId}", currentId)
                       .ToList()[0];
                vm.Comments = _context.Comments
                            .FromSqlRaw($"SELECT * FROM Comments WHERE IdPhotos = {currentId}", currentId)
                            .ToList();
                return View("Details", vm);
            }
            return View("Details",vm);
 
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        

    }
}