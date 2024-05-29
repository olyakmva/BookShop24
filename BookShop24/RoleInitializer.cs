using Microsoft.AspNetCore.Identity;

namespace BookShop24
{
    public static class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@mail.ru";
            string password = "_Boss2024";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await roleManager.FindByNameAsync("manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("manager"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new IdentityUser(adminEmail)
                {
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
