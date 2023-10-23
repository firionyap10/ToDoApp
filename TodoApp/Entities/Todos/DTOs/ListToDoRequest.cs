using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Entities.Enums;

namespace TodoApp.API.Entities.ToDos.DTOs
{
    public class ListToDoRequest : BaseList
    {
        public string? Name { get; set; }

        public List<string>? Tags { get; set; }

        public DateTime? DueDateStart { get; set; }

        public DateTime? DueDateEnd { get; set; }

        public List<ToDoStatus>? Status { get; set; }
    }
}
