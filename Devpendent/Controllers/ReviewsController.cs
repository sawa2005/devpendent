using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Devpendent.Data;
using Devpendent.Models;
using Devpendent.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using SmartBreadcrumbs.Nodes;
using Microsoft.AspNetCore.Authorization;

namespace Devpendent.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly DevpendentContext _context;
        private readonly UserManager<DevpendentUser> _userManager;

        public ReviewsController(DevpendentContext context, UserManager<DevpendentUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reviews
        public async Task<IActionResult> Index(string userName)
        {
            var user = await _context.Users
                .Include(u => u.Reviews)
                .FirstOrDefaultAsync(m => m.UserName == userName);

            if (User.Identity.Name == userName)
            {
                ViewBag.CreateReview = false;
            }

            if (user.Reviews != null)
            {
                ViewBag.ReviewCount = user.Reviews.Count();
            }

            else
            {
                ViewBag.ReviewCount = 0;
            }

            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Username = userName;

            var devpendentContext = _context.Reviews.Where(r => r.UserId == user.Id);

            var userNode = new MvcBreadcrumbNode("Profile", "Users", "@" + userName + "'s Profile") { RouteValues = new { userName } };
            var reviewNode = new MvcBreadcrumbNode("Index", "Reviews", "All Reviews") { Parent = userNode };

            ViewData["BreadcrumbNode"] = reviewNode;

            return View(await devpendentContext.OrderByDescending(r => r.Date).ToListAsync());
        }

        // GET: Reviews/Create
        [Authorize]
        public async Task<IActionResult> Create(string userName)
        {
            var user = await _context.Users
                .Include(u => u.Reviews)
                .FirstOrDefaultAsync(m => m.UserName == userName);

            ViewBag.UserId = user.Id;
            ViewBag.UserName = userName;
            ViewBag.CurrentDate = DateTime.Now;

            var userNode = new MvcBreadcrumbNode("Profile", "Users", "@" + userName + "'s Profile") { RouteValues = new { userName } };
            var reviewNode = new MvcBreadcrumbNode("Index", "Reviews", "All Reviews") { Parent = userNode, RouteValues = new { userName } };
            var createReviewNode = new MvcBreadcrumbNode("Create", "Reviews", "Leave a Review") { Parent = reviewNode };

            ViewData["BreadcrumbNode"] = createReviewNode;

            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Author,Title,Date,Rating,Text,UserId")] Review review)
        {
            if (ModelState.IsValid)
            {
                review.Author = User.Identity.Name;

                _context.Add(review);

                await _context.SaveChangesAsync();

                var user = await _context.Users
                .Include(u => u.Reviews)
                .FirstOrDefaultAsync(m => m.Id == review.UserId);

                return RedirectToAction(nameof(Index), new { userName = user.UserName });
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", review.UserId);
            return View(review);
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
