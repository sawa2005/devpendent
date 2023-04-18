using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Devpendent.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Devpendent.Areas.Identity.Data;

// Add profile data for application users by adding properties to the DevpendentUser class
public class DevpendentUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Location { get; set; }

    [DataType(DataType.Time)]
    [Column(TypeName = "Date")]
    public DateTime RegisterDate { get; set; }

    public string Description { get; set; }
    public string Image { get; set; }
    public ICollection<Job> Jobs { get; set; }
    public ICollection<Education> Educations { get; set; }
    public ICollection<Project> Projects { get; set; }
}

