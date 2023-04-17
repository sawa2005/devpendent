using Devpendent.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Devpendent.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public DevpendentUser User { get; set; }
    }
}
