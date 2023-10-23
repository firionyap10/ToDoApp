using TodoApp.API.Entities.Enums;
using TodoApp.API.Entities.Todos;

namespace TodoApp.API.Entities.ToDos.DTOs
{
    public class GetToDoResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public ToDoStatus Status { get; set; }

        public ToDoPriority Priority { get; set; }

        public List<Tag> Tags { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
