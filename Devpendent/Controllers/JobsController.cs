﻿using System;
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
using SmartBreadcrumbs.Attributes;

namespace Devpendent.Controllers
{
    public class JobsController : Controller
    {
        private readonly DevpendentContext _context;
        private readonly UserManager<DevpendentUser> _userManager;

        public JobsController(DevpendentContext context, UserManager<DevpendentUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Jobs
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userId = claims.Value;

            var accountNode = new MvcRouteBreadcrumbNode("Identity", "Manage your account") { RouteValues = new { id = "" } };
            var jobsNode = new MvcBreadcrumbNode("Index", "Jobs", "ViewData.Title") { Parent = accountNode };

            ViewData["BreadcrumbNode"] = jobsNode;

            var devpendentContext = _context.Jobs.Where(j => j.UserId == userId);
            return View(await devpendentContext.ToListAsync());
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Jobs/Create
        public IActionResult Create()
        {
            var accountNode = new MvcRouteBreadcrumbNode("Identity", "Manage your account") { RouteValues = new { id = "" } };
            var jobsNode = new MvcBreadcrumbNode("Index", "Jobs", "Manage your jobs") { Parent = accountNode };
            var jobNode = new MvcBreadcrumbNode("Index", "Jobs", "Create job") { Parent = jobsNode };

            ViewData["BreadcrumbNode"] = jobNode;

            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,StartDate,EndDate,Description,UserId")] Job job)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                var userId = claims.Value;
                job.UserId = userId;

                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(job);
        }

        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            var accountNode = new MvcRouteBreadcrumbNode("Identity", "Manage your account") { RouteValues = new { id = "" } };
            var jobsNode = new MvcBreadcrumbNode("Index", "Jobs", "Manage your jobs") { Parent = accountNode };
            var jobNode = new MvcBreadcrumbNode("Index", "Jobs", "Edit " + job.Title) { Parent = jobsNode };

            ViewData["BreadcrumbNode"] = jobNode;

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", job.UserId);
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,StartDate,EndDate,Description,UserId")] Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", job.UserId);
            return View(job);
        }

        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Jobs == null)
            {
                return Problem("Entity set 'DevpendentContext.Jobs'  is null.");
            }
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                _context.Jobs.Remove(job);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.Id == id);
        }
    }
}
