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

namespace Devpendent.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly DevpendentContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<DevpendentUser> _userManager;

        public ProjectsController(DevpendentContext context, IWebHostEnvironment webHostEnvironment, UserManager<DevpendentUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string sortOrder, string categorySlug = "", int p = 1)
        {
            int pageSize = 3;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.CategorySlug = categorySlug;

            if (sortOrder != null)
            {
                //if sortOrder is not null, update the session to store the new sort order
                HttpContext.Session.SetString("sortOrder", sortOrder);
            }
            else if (HttpContext.Session.GetString("sortOrder") != null)
            {
                sortOrder = HttpContext.Session.GetString("sortOrder");
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSort = "title_sort";
            ViewBag.BudgetSort = "budget_sort";

            if (categorySlug == "")
            {
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Projects.Count() / pageSize);

                var projects = from x in _context.Projects select x;

                switch (sortOrder)
                {
                    default:
                    case "title_sort":
                        projects = projects.OrderBy(p => p.Title);
                        break;

                    case "budget_sort":
                        projects = projects.OrderBy(p => p.Budget);
                        break;
                }

                return View(await projects.Skip((p - 1) * pageSize).Take(pageSize).ToListAsync());
            }

            Category category = await _context.Categories.Where(c => c.Slug == categorySlug).FirstOrDefaultAsync();

            if (category == null) return RedirectToAction("Index");

            var projectsByCategory = _context.Projects.Where(p => p.CategoryId == category.Id);
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)projectsByCategory.Count() / pageSize);

            switch (sortOrder)
            {
                default:
                case "title_sort":
                    projectsByCategory = projectsByCategory.OrderBy(p => p.Title);
                    break;

                case "budget_sort":
                    projectsByCategory = projectsByCategory.OrderBy(p => p.Budget);
                    break;
            }

            return View(await projectsByCategory.Skip((p - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        // GET: Projects
        public async Task<IActionResult> Manage()
        {
            var dataContext = _context.Projects.Include(p => p.Category);
            return View(await dataContext.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");

            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Slug,Title,Description,Budget,DeliveryTime,Image,ImageUpload,CategoryId,UserId")] Project project)
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", project.CategoryId);

            if (ModelState.IsValid)
            {
                project.CreationDate = DateTime.Now;

                project.Slug = project.Title.ToLower().Replace(" ", "-");

                var slug = await _context.Projects.FirstOrDefaultAsync(p => p.Slug == project.Slug);

                if (slug != null)
                {
                    project.Slug += project.Id;
                }

                if (project.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/projects");
                    string imageName = Guid.NewGuid().ToString() + "_" + project.ImageUpload.FileName;

                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await project.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    project.Image = imageName;
                }

                var userId = _userManager.GetUserId(User);
                project.UserId = userId;

                _context.Add(project);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The project has been created!";

                return RedirectToAction(nameof(Index));
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", project.CategoryId);

            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project input)
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", input.CategoryId);

            if (ModelState.IsValid)
            {
                var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);

                if (project == null) 
                {
                    return NotFound();
                }

                input.Slug = input.Title.ToLower().Replace(" ", "-");

                if (input.Slug != project.Slug)
                {
                    project.Slug = input.Slug;
                }

                project.Title = input.Title;
                project.Budget = input.Budget;
                project.Description = input.Description;
                project.DeliveryTime = input.DeliveryTime;

                if (input.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/projects");
                    string imageName = Guid.NewGuid().ToString() + "_" + input.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    if (project.Image != null)
                    {
                        string oldImagePath = Path.Combine(uploadsDir, project.Image);

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await input.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    project.Image = imageName;
                }

                project.CategoryId = input.CategoryId;

                _context.Projects.Update(project);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The project has been edited!";

                return RedirectToAction(nameof(Index));
            }

            return View(input);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'DataContext.Projects'  is null.");
            }

            var project = await _context.Projects.FindAsync(id);

            if (project != null)
            {
                if(!string.Equals(project.Image, "noimage.png"))
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/projects");
                    string oldImagePath = Path.Combine(uploadsDir, project.Image);

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _context.Projects.Remove(project);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "The project has been deleted!";

            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
