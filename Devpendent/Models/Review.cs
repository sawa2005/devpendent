using Devpendent.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Devpendent.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public DevpendentUser User { get; set; }
    }
}
