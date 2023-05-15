using NuGet.DependencyResolver;

namespace WebApplication1.Models
{
    public class ViewModel
    {
        public List<Comments> Comments { get; set; }
        public Photos photo { get; set; }

        public String newComment { get; set; }

    }
}


