using FluentValidation;
using TodoApp.API.Entities.ToDos.DTOs;

namespace TodoApp.API.Entities.Authentications
{
    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name date is required.");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of birth is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.").MinimumLength(4).WithMessage("Password minimum length 4");
        }
    }
}
