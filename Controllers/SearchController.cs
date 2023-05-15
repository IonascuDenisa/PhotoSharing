using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class SearchController : Controller
    {
        [HttpPost, ActionName("Search")]
        public ActionResult Search(string searchString)
        {
            System.Diagnostics.Debug.Write("@@@@@@@@@@@@@");
            System.Diagnostics.Debug.Write(searchString);
            System.Diagnostics.Debug.Write("*******************");
            // List<Photos> PhotosList = new List<Photos>();
            //PhotosList = _context.Photos
            // .FromSqlRaw($"SELECT * FROM Comments WHERE Title = {searchString}", searchString)
            //.ToList();

            return View();
        }
    }
}
