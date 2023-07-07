using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Devpendent.Data;
using Devpendent.Models;
using Microsoft.AspNetCore.Hosting;
using SmartBreadcrumbs.Attributes;

namespace Devpendent.Controllers
{
    [Breadcrumb("Categories")]
    public class CategoriesController : Controller
    {
        private readonly DevpendentContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoriesController(DevpendentContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        [Breadcrumb(Title = "Manage")]
        public async Task<IActionResult> Manage()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        [Breadcrumb(FromAction = "Manage", Title = "Details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Breadcrumb(FromAction = "Manage", Title = "Create category")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Slug,Name,Description,Image,ImageUpload")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/categories");
                    var extension = Path.GetExtension(category.ImageUpload.FileName);
                    string imageName = category.Slug + extension;

                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await category.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    category.Image = imageName;
                }

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Manage));
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        [Breadcrumb(FromAction = "Manage", Title = "Edit category")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Slug,Name,Description,Image,ImageUpload")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (category.ImageUpload != null)
                    {
                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/categories");
                        var extension = Path.GetExtension(category.ImageUpload.FileName);
                        string imageName = category.Slug + extension;

                        string filePath = Path.Combine(uploadsDir, imageName);

                        if (category.Image != null)
                        {
                            string oldImagePath = Path.Combine(uploadsDir, category.Image);

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await category.ImageUpload.CopyToAsync(fs);
                        fs.Close();

                        category.Image = imageName;
                    }

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Manage));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        [Breadcrumb(FromAction = "Manage", Title = "Delete category")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'DevpendentContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
