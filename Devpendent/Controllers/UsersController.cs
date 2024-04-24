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
using System.Security.Claims;
using SmartBreadcrumbs.Nodes;

namespace Devpendent.Controllers
{
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
                .Include(u => u.Reviews)
                .FirstOrDefaultAsync(m => m.UserName == userName);

            if (user == null)
            {
                return NotFound();
            }

            if (user.Projects != null)
            {
                ViewBag.ProjectCount = user.Projects.Count();
            }

            else
            {
                ViewBag.ProjectCount = 0;
            }

            if (user.Reviews != null)
            {
                ViewBag.ReviewCount = user.Reviews.Count();
            }

            else
            {
                ViewBag.ReviewCount = 0;
            }

            var userNode = new MvcBreadcrumbNode("Profile", "Users", "ViewData.Title");

            ViewData["BreadcrumbNode"] = userNode;

            return View(user);
        }

        public async Task<IActionResult> Projects(string userName, string sortOrder, string searchString, string categorySlug = "", int p = 1)
        {
            int pageSize = 3;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.CategorySlug = categorySlug;
            ViewBag.Username = userName;

            string[] sortOptions = { "Title", "Budget" };
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortOptions = sortOptions;

            if (sortOrder != null)
            {
                //if sortOrder is not null, update the session to store the new sort order
                HttpContext.Session.SetString("sortOrder", sortOrder);
            }
            else if (HttpContext.Session.GetString("sortOrder") != null)
            {
                sortOrder = HttpContext.Session.GetString("sortOrder");
            }

            var projects = from x in _context.Projects.Where(p => p.User.UserName == userName) select x;

            if (!string.IsNullOrEmpty(searchString))
            {
                projects = projects.Where(p => p.Title.Contains(searchString));
            }

            if (categorySlug == "")
            {
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Projects.Count() / pageSize);

                switch (sortOrder)
                {
                    default:
                    case "Title":
                        projects = projects.OrderBy(p => p.Title);
                        break;

                    case "Budget":
                        projects = projects.OrderBy(p => p.Budget);
                        break;
                }

                ViewBag.ProjectCount = projects.Count();

                var userNode = new MvcBreadcrumbNode("Profile", "Users", "@" + userName + "'s Profile") { RouteValues = new { userName } };

                var projectsNode = new MvcBreadcrumbNode("Index", "Users", "Projects")
                {
                    RouteValues = new { userName },
                    Parent = userNode
                };

                ViewData["BreadcrumbNode"] = projectsNode;

                return View(await projects.Include(p => p.User).Skip((p - 1) * pageSize).Take(pageSize).ToListAsync());
            }

            Category category = await _context.Categories.Where(c => c.Slug == categorySlug).FirstOrDefaultAsync();

            ViewBag.CategoryName = category.Name;

            if (category == null) return RedirectToAction("Index");

            var projectsByCategory = _context.Projects.Where(p => p.CategoryId == category.Id);
            var projectsByCategoryUser = projectsByCategory.Where(p => p.User.UserName == userName);
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)projectsByCategoryUser.Count() / pageSize);

            switch (sortOrder)
            {
                default:
                case "title_sort":
                    projectsByCategoryUser = projectsByCategoryUser.OrderBy(p => p.Title);
                    break;

                case "budget_sort":
                    projectsByCategoryUser = projectsByCategoryUser.OrderBy(p => p.Budget);
                    break;
            }

            ViewBag.ProjectCount = projectsByCategoryUser.Count();

            return View(await projectsByCategoryUser.Include(p => p.User).Skip((p - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        [HttpPost]
        public IActionResult SetSort(string sort)
        {
            if (sort == null)
            {
                return View();
            }

            HttpContext.Session.SetString("sortOrder", sort);

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult GetSort()
        {
            return Json(HttpContext.Session.GetString("sortOrder"));
        }
    }
}
