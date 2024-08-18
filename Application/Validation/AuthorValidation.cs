using Domain.Entities;
using FluentValidation;

namespace Application.Validation
{
    public class AuthorValidation : AbstractValidator<Author>
    {
        public AuthorValidation()
        {
            RuleFor(author => author.FullName)
                .NotEmpty();

            RuleFor(author => author.BirthDate)
                .NotEmpty()
                .Must(author => author < DateOnly.FromDateTime(DateTime.Now).AddYears(10))
                .WithMessage("Date cannot be upper today");
        }
    }
}
