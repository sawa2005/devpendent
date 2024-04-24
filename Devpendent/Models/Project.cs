using Devpendent.Areas.Identity.Data;
using Devpendent.Data.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Devpendent.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Slug { get; set; }

        public DateTime CreationDate { get; set; }
        
        [Required(ErrorMessage = "Please enter a title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a budget for your project")]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Budget { get; set; }

        [Required(ErrorMessage = "Please enter a timeframe within which your project will be completed")]
        public string DeliveryTime { get; set; }

        public string Image { get; set; }

        [NotMapped]
        [FileExtension]
        [Required(ErrorMessage = "Please upload a cover image")]
        public IFormFile ImageUpload { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please choose a category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string UserId { get; set; }

        public DevpendentUser User { get; set; }
    }
}
