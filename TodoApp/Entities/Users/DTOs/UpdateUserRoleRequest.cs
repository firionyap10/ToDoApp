using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TodoApp.API.Entities.Users.DTOs
{
    public class UpdateUserRoleRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        public List<string> RoleNames { get; set; }
    }
}
