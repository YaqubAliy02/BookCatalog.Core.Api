using Application.DTOs.UserDTO;
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

            if (user is not null)
            {
                string newPassword = request.CurrentPassword.GetHash();
                if (newPassword == user.Password
                    && request.NewPassword == request.ConfirmNewPassword)
                {
                    user.Password = request.NewPassword.GetHash();
                    await _userRepository.UpdateAsync(user);

                    return new OkObjectResult("Password is changed successfully✅");
                }
                else return new BadRequestObjectResult("Incorrect password❌");
            }

            return new BadRequestObjectResult("User is not found");
        }
    }
}
