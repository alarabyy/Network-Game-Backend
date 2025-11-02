using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DataAccess.Repositories;
using DataAccess.Interfaces;
using DataAccess.Entities;
using DataAccess.Seeds;

namespace DataAccess.Extensions;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDataAccessConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        // Register Identity services
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            // --- DEVELOPMENT ONLY PASSWORD SETTINGS ---
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequiredLength = 5; // Set a lower required length
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();
        
        // register repositories
        services.AddScoped<IFeedSourceRepository, FeedSourceRepository>();
        services.AddScoped<IFeedItemRepository, FeedItemRepository>();
        services.AddScoped<ISubscriberRepository, SubscriberRepository>();
        services.AddScoped<IWriterApplicationRequestRepository, WriterApplicationRequestRepository>();

        return services;
    }
    
    public static async Task SeedIdentityDataAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await IdentitySeeder.SeedRolesAsync(roleManager);
        await IdentitySeeder.SeedAdminAsync(userManager);
    }
}