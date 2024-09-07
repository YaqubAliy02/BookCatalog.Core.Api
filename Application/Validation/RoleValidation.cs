
using Domain.Entities;
using FluentValidation;

namespace Application.Validation
{
    public class RoleValidation : AbstractValidator<Role>
    {
        public RoleValidation()
        {
            RuleFor(role => role.RoleName).NotEmpty().WithMessage("Role name is required.");
        }
    }
}
