using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Abstraction;
using Application.DTOs.UserDTO;
using Application.Models;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Accounts.Command
{
    public class RegisterUserCommand : IRequest<ResponseCore<RegisterUserCommandResult>>
    {
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }
        public Guid[] RolesId { get; set; }
    }
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResponseCore<RegisterUserCommandResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly ITokenService _tokenService;
        private readonly IValidator<User> _validator;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IRoleRepository roleRepository,
            ITokenService tokenService,
            IValidator<User> validator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _tokenService = tokenService;
            _validator = validator;
        }

        public async Task<ResponseCore<RegisterUserCommandResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var result = new ResponseCore<RegisterUserCommandResult>();
            User user = _mapper.Map<User>(request);
            var validationResult = _validator.Validate(user);

            if (!validationResult.IsValid)
            {
                result.ErrorMessage = validationResult.Errors.ToArray();
                result.StatusCode = 400;

                return result;
            }

            if (user is null)
            {
                result.ErrorMessage = new string[] { "User is not found" };
                result.StatusCode = 400;

                return result;
            }

            List<Role> permissions = new();
            if (user.Roles is not null)
            {
                for (int i = 0; i < user.Roles.Count; i++)
                {
                    Role permission = user.Roles.ToArray()[i];

                    permission = await _roleRepository.GetByIdAsync(permission.RoleId);

                    if (permission is null)
                    {
                        result.ErrorMessage = new string[] { "Book Id: " + permission.RoleId + "Not found " };
                        result.StatusCode = 400;

                        return result;
                    }
                    permissions.Add(permission);
                }
            }

            user.Roles = permissions;

            if (request is not null)
            {
                RegisteredUserDTO userDTO = new()
                {
                    User = user,
                    UserTokens = await _tokenService.CreateTokensAsync(user)
                };
            }
            user = await _userRepository.AddAsync(user);

            result.Result = _mapper.Map<RegisterUserCommandResult>(user);
            result.StatusCode = 200;

            return result;
        }
    }

    public class RegisterUserCommandResult
    {
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }
        public Guid[] RolesId { get; set; }
    }
}
