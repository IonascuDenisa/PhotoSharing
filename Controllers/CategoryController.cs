using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new CategoryPhoto
            {
                category = new Category()
            };
            return View(model);
        }


        [HttpPost]
        public IActionResult GetPhotos(string category, string categories)
        {
            var selectedCategory = category;
            var allCategories = categories;
            var a = 1;

            var photos = _context.Photos
                .Where(p => p.Category == selectedCategory)
                .ToList();

            var model = new CategoryPhoto
            {
                category = new Category(),
                photos = photos
            };
            return View("Index", model);

        }

    }
}
