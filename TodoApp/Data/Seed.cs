using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using TodoApp.API.Entities.Users;

namespace TodoApp.API.API.Data
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                //Source: https://www.json-generator.com/

                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                var roles = new List<Role>
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "Manager" },
                    new Role { Name = "Standard" },
                };

                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                    user.CreatedDate = DateTime.UtcNow;
                    _userManager.CreateAsync(user, "password").Wait();
                    _userManager.AddToRoleAsync(user, "Standard").Wait();
                }

                var adminUser = new User
                {
                    UserName = "firionyap",
                    FirstName = "Firion",
                    LastName = "Yap",
                    DateOfBirth = new DateTime(1992, 10, 23),
                    CreatedDate = DateTime.UtcNow
                };

                var result = _userManager.CreateAsync(adminUser, "password").Result;

                if (result.Succeeded)
                {
                    var admin = _userManager.FindByNameAsync("firionyap").Result;
                    _userManager.AddToRolesAsync(admin, new[] { "Admin", "Manager", "Standard" }).Wait();
                }
            }
        }
    }
}
