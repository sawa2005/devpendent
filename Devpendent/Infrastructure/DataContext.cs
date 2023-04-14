using Devpendent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Devpendent.Infrastructure
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
