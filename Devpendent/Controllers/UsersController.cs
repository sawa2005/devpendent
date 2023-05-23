using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Devpendent.Infrastructure;
using Devpendent.Models;
using Devpendent.Data;
using Microsoft.AspNetCore.Identity;
using Devpendent.Areas.Identity.Data;
using System.Drawing.Printing;
using Microsoft.Data.SqlClient;
using Devpendent.Filters;
using System.Security.Claims;

namespace Devpendent.Controllers
{
    [BreadcrumbActionFilter]
    public class UsersController : Controller
    {
        private readonly DevpendentContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<DevpendentUser> _userManager;

        public UsersController(DevpendentContext context, IWebHostEnvironment webHostEnvironment, UserManager<DevpendentUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public async Task<IActionResult> Profile(string userName)
        {
            if (userName == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Projects)
                .Include(u => u.Jobs)
                .Include(u => u.Educations)
                .FirstOrDefaultAsync(m => m.UserName == userName);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}
