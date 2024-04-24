using Devpendent.Areas.Identity.Data;
using Devpendent.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace Devpendent.Data;

public class DevpendentContext : IdentityDbContext<DevpendentUser>
{
    public DevpendentContext(DbContextOptions<DevpendentContext> options) : base(options) {}

    public DbSet<Project> Projects { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<DevpendentUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<DevpendentUser>()
            .HasMany(e => e.Jobs)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .IsRequired();

        builder.Entity<DevpendentUser>()
            .HasMany(e => e.Reviews)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .IsRequired();

        builder.Entity<DevpendentUser>()
            .HasMany(e => e.Educations)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .IsRequired();

        builder.Entity<DevpendentUser>()
            .HasMany(e => e.Projects)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .IsRequired();
    }
}
