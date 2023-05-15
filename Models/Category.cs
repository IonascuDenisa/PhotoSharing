using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace WebApplication1.Models
{
    public class Category
    {
        public List<string> Categories { get; set; }

        public Category()
        {
            Categories = new List<string>() { "landscape", "nature", "animals", "food&drinks", "arhitecture" };
        }
    }
}
