using System.ComponentModel.DataAnnotations.Schema;
using TodoApp.API.Entities.ToDos;

namespace TodoApp.API.Entities.Todos
{
    public class Tag
    {
        public int Id { get; set; }

        public int ToDoId { get; set; }

        public string Name { get; set; }
    }
}
