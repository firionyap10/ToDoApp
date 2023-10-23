using TodoApp.API.Entities.Enums;

namespace TodoApp.API.Entities.Users.DTOs
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public DateTime UpdateDate { get; protected set; } = DateTime.UtcNow;
    }
}
