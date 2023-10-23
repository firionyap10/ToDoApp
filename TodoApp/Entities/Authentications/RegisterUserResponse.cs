using Microsoft.AspNetCore.Identity;

namespace TodoApp.API.Entities.Authentications
{
    public class RegisterUserResponse
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public IEnumerable<IdentityError>? Errors { get; set; }
    }
}
