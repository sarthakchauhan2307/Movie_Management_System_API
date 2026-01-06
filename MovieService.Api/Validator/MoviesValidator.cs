using FluentValidation;
using MovieService.Api.Models;

namespace MovieService.Api.Validator
{
    public class MoviesValidator : AbstractValidator<Movies>
    {
        public MoviesValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title not be empty");

            RuleFor(x => x.Actor)
                .NotEmpty()
                .WithMessage("add Actor name");
        }
    }
}
