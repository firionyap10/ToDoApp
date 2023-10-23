namespace TodoApp.API.Entities.Users.DTOs
{
    public class UpdateUserRoleResponse
    {
        public int Id { get; set; }
        public List<string> RoleNames { get; set; }
    }
}
