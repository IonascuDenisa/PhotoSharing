using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace WebApplication1.Models
{
    public class Comments    {
        [Key]
        public int IdComment { get; set; }
        public int IdPhotos { get; set; }
        [Required]
        public string Text { get; set; }
        [ValidateNever]
        public string? DateAdded { get; set; } = DateTime.Now.ToString();


    }
}
