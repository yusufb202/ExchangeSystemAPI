using Core.Models;
using Microsoft.AspNetCore.Identity;
using Repositories;

public static class DbInitializer
{
    public static async Task SeedAdminUser(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        const string adminUserName = "admin";
        const string adminEmail = "admin@gmail.com";
        const string adminPassword = "Admin.123";

        if (await userManager.FindByNameAsync(adminUserName) == null)
        {
            var adminUser = new User { UserName = adminUserName, Email = adminEmail };
            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}







