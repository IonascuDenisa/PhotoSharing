using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LogInController : Controller
    {
        private readonly ApplicationDbContext _db;

        public LogInController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<User> objLogInList = _db.Users.ToList();
            
            return View(objLogInList);
        }
        public IActionResult LogIn(User obj)
        {
            System.Diagnostics.Debug.WriteLine(obj.Email);
            System.Diagnostics.Debug.WriteLine(_db.Users);
            foreach(var user in _db.Users){
                if (user.Email == obj.Email && user.Password == obj.Password)
                {
                    System.Diagnostics.Debug.WriteLine("este in if################");

                }
            }
            return View();
        }
    }
}
