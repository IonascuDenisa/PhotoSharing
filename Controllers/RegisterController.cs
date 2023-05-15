using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
  
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RegisterController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult SuccesfullySignUp()
        {

           
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User obj)
        {
            
            
            if (ModelState.IsValid)
            {
                _db.Users.Add(obj);
                
                _db.SaveChanges();
                return RedirectToAction("SuccesfullySignUp");
            }

            return View(obj);
            
        }


    }
}


    