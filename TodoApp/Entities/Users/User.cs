using Microsoft.AspNetCore.Identity;
using TodoApp.API.Entities.Enums;

namespace TodoApp.API.Entities.Users
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<UserRole> UserRoles { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
