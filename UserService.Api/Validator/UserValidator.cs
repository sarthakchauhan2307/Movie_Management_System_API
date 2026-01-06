using FluentValidation;
using UserService.Api.Model;

namespace UserService.Api.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Email Is required");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Length(10)
                .WithMessage("Mobile number must be 10 digit and not empty");

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("UserName is Required");


        }
    }
}
