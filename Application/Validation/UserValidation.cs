using System.Text.RegularExpressions;
using Domain.Entities;
using FluentValidation;
using StackExchange.Redis;

namespace Application.Validation
{
    public class UserValidation : AbstractValidator<User>
    { 
     
        public UserValidation()
        {
            RuleFor(user => user.FullName).NotEmpty().WithMessage("Role name is required.");
            RuleFor(user => user.Roles).NotEmpty().WithMessage($"{nameof(User)}.roles");
            RuleFor(user => user.Email).NotEmpty().EmailAddress().WithMessage("Please provide a valid email address in the format 'example@example.com'.");
        }
    }
}
