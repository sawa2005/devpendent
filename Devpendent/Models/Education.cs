using Devpendent.Areas.Identity.Data;

namespace Devpendent.Models
{
    public class Education
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UniversityName { get; set; }
        public int GraduationYear { get; set; }
        public string UserId { get; set; }
        public DevpendentUser User { get; set; }
    }
}
