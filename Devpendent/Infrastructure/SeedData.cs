using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using Devpendent.Models;
using Devpendent.Data;

namespace Devpendent.Infrastructure
{
    public class SeedData
    {
        public static void SeedDatabase(DevpendentContext context)
        {
            context.Database.Migrate();

            if (!context.Projects.Any())
            {
                Category webdev = new Category 
                { 
                    Name = "Web Development", 
                    Slug = "web-development", 
                    Description = "Bring your ideas to life over the internet.", 
                    Image = "webdev.jpg" 
                };

                context.Projects.AddRange(
                    new Project
                    {
                        Slug = "example-project",
                        Title = "Example Project",
                        Description = "This is an example project created for development purposes.",
                        Budget = 100M,
                        DeliveryTime = "7 Days",
                        Image = "example.jpg",
                        Category = webdev
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
