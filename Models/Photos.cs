using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebApplication1.Models
{
    public class Photos
    {
        [Key]
        public int Id { get; set; }

        public string Category { get; set; }
        
        public string Description { get; set; }

        [ValidateNever]
        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("ImagePath")]
        public string ImagePath { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }

        [ValidateNever]
        public string? DateAdded { get; set; } = DateTime.Now.ToString();


    }

    public class ImageMetadata
    {
        public string Category { get; set; }
        public string Description { get; set; }
    }


}
