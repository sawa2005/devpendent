using Devpendent.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Devpendent.Infrastructure.Components
{
    public class RatingsViewComponent : ViewComponent
    {
        private readonly DevpendentContext _context;

        public RatingsViewComponent(DevpendentContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id, string type, string userName)
        {
            if (type == "average")
            {
                var reviews = await _context.Reviews.Where(r => r.User.UserName == userName).ToListAsync();

                var rating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

                ViewBag.ReviewCount = reviews.Count();

                return View("Average", rating);
            }

            else if (type == "small")
            {
                var reviews = await _context.Reviews.Where(r => r.User.UserName == userName).ToListAsync();

                var rating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

                ViewBag.ReviewCount = reviews.Count();

                return View("Small", rating);
            }

            else
            {
                var reviews = await _context.Reviews.Where(r => r.Id == id).ToListAsync();

                var rating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

                return View("Single", rating);
            }
        }
    }
}
