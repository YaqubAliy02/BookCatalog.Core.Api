using Application.Extensions;
using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Users.Command
{
    public class ChangeUserPasswordCommand : IRequest<IActionResult>
    {
        public Guid UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
    public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, IActionResult>
    {
        private readonly IUserRepository _userRepository;

        public ChangeUserPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user is null)
                return new NotFoundObjectResult("User not found");

            if (user.Password != request.CurrentPassword.GetHash())
                return new BadRequestObjectResult("Current password is incorrect");

            if (request.NewPassword != request.ConfirmNewPassword)
                return new BadRequestObjectResult("New password and confirmation do not match");

            user.Password = request.NewPassword.GetHash();
            await _userRepository.UpdatePasswordAsync(user);
            return new OkObjectResult("Password changed successfully ✅");
        }
    }
}
