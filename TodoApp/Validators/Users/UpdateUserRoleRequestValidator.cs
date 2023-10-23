using FluentValidation;
using TodoApp.API.Entities.Users.DTOs;

namespace TodoApp.API.Validators.Todos
{
    public class UpdateUserRoleRequestValidator : AbstractValidator<UpdateUserRoleRequest>
    {
        public UpdateUserRoleRequestValidator()
        {
            RuleFor(x => x.RoleNames).NotEmpty().WithMessage("Role name is required.");
        }
    }
}
