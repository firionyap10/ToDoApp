using FluentValidation;
using TodoApp.API.Entities.ToDos.DTOs;

namespace TodoApp.API.Validators.Todos
{
    public class CreateToDoRequestValidator : AbstractValidator<CreateToDoRequest>
    {
        public CreateToDoRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.DueDate).NotEmpty().WithMessage("Due date is required.");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required.");
            RuleFor(x => x.Priority).NotEmpty().WithMessage("Priority is required.");
        }
    }
}
