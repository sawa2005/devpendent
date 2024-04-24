using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Devpendent.Data;
using Devpendent.Models;
using System.Security.Claims;
using Devpendent.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using SmartBreadcrumbs.Nodes;
using Microsoft.AspNetCore.Authorization;
using SmartBreadcrumbs.Attributes;
using Devpendent.Areas.Identity.Pages.Account.Manage;

namespace Devpendent.Controllers
{
    [Authorize]
    [Breadcrumb("Manage your educations", FromPage = typeof(IndexModel))]
    public class EducationsController : Controller
    {
        private readonly DevpendentContext _context;
        private readonly UserManager<DevpendentUser> _userManager;

        public EducationsController(DevpendentContext context, UserManager<DevpendentUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Educations
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userId = claims.Value;

            var devpendentContext = _context.Educations.Where(j => j.UserId == userId);
            return View(await devpendentContext.ToListAsync());
        }

        // GET: Educations/Create
        [Breadcrumb("Create education")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Educations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,UniversityName,GraduationYear,UserId")] Education education)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                var userId = claims.Value;
                education.UserId = userId;

                _context.Add(education);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(education);
        }

        // GET: Educations/Edit/5
        [Breadcrumb("ViewData.Title")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Educations == null)
            {
                return NotFound();
            }

            var education = await _context.Educations.FindAsync(id);
            if (education == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", education.UserId);

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userId = claims.Value;

            if (education.UserId == userId)
            {
                return View(education);
            }

            else
            {
                return NotFound();
            }
        }

        // POST: Educations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,UniversityName,GraduationYear,UserId")] Education education)
        {
            if (id != education.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(education);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EducationExists(education.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", education.UserId);
            return View(education);
        }

        // GET: Educations/Delete/5
        [Breadcrumb("Delete?")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Educations == null)
            {
                return NotFound();
            }

            var education = await _context.Educations
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (education == null)
            {
                return NotFound();
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userId = claims.Value;

            if (education.UserId == userId)
            {
                return View(education);
            }

            else
            {
                return NotFound();
            }
        }

        // POST: Educations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Educations == null)
            {
                return Problem("Entity set 'DevpendentContext.Educations'  is null.");
            }
            var education = await _context.Educations.FindAsync(id);
            if (education != null)
            {
                _context.Educations.Remove(education);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EducationExists(int id)
        {
            return _context.Educations.Any(e => e.Id == id);
        }
    }
}
