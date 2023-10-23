using TodoApp.API.Entities.Commons.Operations;

namespace TodoApp.API.Entities.Users.DTOs
{
    public class ListUserRequest : BaseList
    {
        public string Name { get; set; }
    }
}
