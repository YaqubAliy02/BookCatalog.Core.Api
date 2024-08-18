using Domain.Entities;
using FluentValidation;

namespace Application.Validation
{
    public class BookValidation : AbstractValidator<Book>
    {
        public BookValidation()
        {
            RuleFor(book => book.Name).NotEmpty()
                .MaximumLength(50)
                .MinimumLength(3)
                .NotNull()
                .WithMessage("Book cannot be null or empty!!!");

            RuleFor(book => book.PublishedDate).NotEmpty()
                .NotNull()
                .Must(x => x < DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Date cannot be upper today");
        }
    }
}
