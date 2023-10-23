using Microsoft.AspNetCore.Identity;
using System.Data;

namespace TodoApp.API.Entities.Users
{
    public class UserRole : IdentityUserRole<int>
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
