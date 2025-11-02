using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Seeds;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        string[] roles = ["Admin", "Writer", "Reader"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new ApplicationRole(role));
        }
    }

    public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
    {
        var adminEmail = "admin@globaltruthwatch.com";
        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

        if (existingAdmin != null)
        {
            return;
        }
        var admin = new ApplicationUser
        {
            UserName = "admin",
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, "admin");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}