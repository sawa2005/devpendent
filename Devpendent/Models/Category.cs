using Devpendent.Data.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Devpendent.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }
    }
}
