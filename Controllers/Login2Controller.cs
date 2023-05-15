using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    
    public class Login2Controller : Controller
    {
        private readonly ApplicationDbContext _db;

        public Login2Controller(ApplicationDbContext db)
        {
            _db = db;
        }
        // Get Action
        public IActionResult Login2()

        {
            Console.WriteLine("Values: {0} {1} ");
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        //Post Action
        [HttpPost]
        public ActionResult Login2(User obj)
        {
         
            if (HttpContext.Session.GetString("UserName") == null)
            {
                
                    
                    var u = _db.Users
                           .Where(a => a.Name.Equals(obj.Name) && a.Password.Equals(obj.Password)).FirstOrDefault();
               
                if (u != null)
                    {
                        HttpContext.Session.SetString("UserName",u.Name.ToString());

                        if (u.Type != null)
                        {
                            HttpContext.Session.SetString("Type", u.Type.ToString());
                        }
                        return RedirectToAction( "Index", "Home");

                    

                }
                  
                }
            else
            {
                return RedirectToAction("Home","Index");
            }
            return View();
        }
        public ActionResult Logout()
        {

            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");

            return RedirectToAction("Login2");
        }
    }
    }

