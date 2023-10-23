namespace TodoApp.API.Entities.Users.DTOs
{
    public class ListUserResponse
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
