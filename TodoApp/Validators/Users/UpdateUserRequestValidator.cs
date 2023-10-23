using FluentValidation;
using TodoApp.API.Entities.Users.DTOs;

namespace TodoApp.API.Validators.Todos
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name date is required.");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of birth is required.");
        }
    }
}
