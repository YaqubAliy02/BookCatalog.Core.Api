using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Abstraction;
using Application.DTOs.UserDTO;
using Application.Extensions;
using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Accounts.Command
{
    public class LoginUserCommand : IRequest<IActionResult>
    {
        [EmailAddress]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }
    }
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<IActionResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = (await _userRepository.GetAsync(x =>
               x.Password == request.Password.GetHash() &&
               x.Email == request.Email)).FirstOrDefault();

            if (user is not null)
            {
                RegisteredUserDTO userDTO = new()
                {
                    User = user,
                    UserTokens = await _tokenService.CreateTokensAsync(user)
                };

                return new OkObjectResult(userDTO);
            }
            return new BadRequestObjectResult("Email or Password is incorrect!!!");
        }
    }
}
