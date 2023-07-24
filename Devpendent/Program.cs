using Devpendent.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Devpendent.Data;
using Devpendent.Areas.Identity.Data;
using SmartBreadcrumbs.Extensions;
using System.Reflection;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDistributedMemoryCache();

        #if DEBUG
            builder.Services.AddSassCompiler();
        #endif

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.IsEssential = true;
        });

        builder.Services.AddDbContext<DevpendentContext>(options =>
        {
            options.UseSqlServer(builder.Configuration["ConnectionStrings:DbConnection"]);
        });

        builder.Services.AddDefaultIdentity<DevpendentUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DevpendentContext>();

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddBreadcrumbs(Assembly.GetExecutingAssembly(), options =>
        {
            options.TagName = "nav";
            options.TagClasses = "";
            options.OlClasses = "breadcrumb";
            options.LiClasses = "breadcrumbs-item";
            options.ActiveLiClasses = "breadcrumb-item active";
            options.SeparatorElement = "<li class=\"separator\"><svg xmlns=\"http://www.w3.org/2000/svg\" width=\"5.186\" height=\"9.073\" viewBox=\"0 0 5.186 9.073\"><path d=\"M13.5,16.234l3.617-3.617L13.5,9\" transform=\"translate(-12.581 -8.081)\" fill=\"none\" stroke=\"#777\" stroke-linecap=\"round\" stroke-linejoin=\"round\" stroke-width=\"1.3\"/></svg></li>";
        });

        var app = builder.Build();

        app.UseSession();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSession();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
            );

        app.MapControllerRoute(
                name: "Identity",
                pattern: "/Identity/Account/Manage"
            );

        app.MapRazorPages();

        // Create Admin Roles
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var role = "Administrator";

            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Assign Admin Roles to Admin User
        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<DevpendentUser>>();

            string email = "admin@devpendent.com";
            string password = "8L%N2eQt0s41";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new DevpendentUser();
                user.UserName = "admin";
                user.Email = email;
                user.EmailConfirmed = true;

                await userManager.CreateAsync(user, password);

                await userManager.AddToRoleAsync(user, "Administrator");
            }
        }

        app.Run();
    }
}

#if DEBUG

#endif
