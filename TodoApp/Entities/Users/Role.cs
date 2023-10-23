using Microsoft.AspNetCore.Identity;

namespace TodoApp.API.Entities.Users
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
