﻿using Devpendent.Data;
using Devpendent.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Devpendent.Infrastructure.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly DevpendentContext _context;

        public CategoriesViewComponent(DevpendentContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string view)
        {
            if (view == "Links")
            {
                return View("Links", await _context.Categories.ToListAsync());
            }

            if (view == "Buttons")
            {
                return View("Buttons", await _context.Categories.ToListAsync());
            }

            return View("Default", await _context.Categories.ToListAsync());
        }
    }
}
