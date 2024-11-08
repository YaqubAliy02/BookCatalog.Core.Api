using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Models;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Users.Command
{
    public class CreateUserCommand : IRequest<ResponseCore<CreateUserCommandHandlerResult>>
    {
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }
        public Guid[] RolesId { get; set; }
    }   
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseCore<CreateUserCommandHandlerResult>>
    {
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<User> _validator;

        public CreateUserCommandHandler(
            IMapper mapper,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IValidator<User> validator)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<ResponseCore<CreateUserCommandHandlerResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = new ResponseCore<CreateUserCommandHandlerResult>();
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
            List<Role> roles = new();
            for (int i = 0; i < user.Roles.Count; i++)
            {
                Role role = user.Roles.ToArray()[i];
                role = await _roleRepository.GetByIdAsync(role.RoleId);

                if (role is null)
                {
                    result.ErrorMessage = new string[] { "Book Id: " + role.RoleId + "Not found " };
                    result.StatusCode = 400;

                    return result;
                }
                roles.Add(role);
            }
            user.Roles = roles;

            user = await _userRepository.AddAsync(user);

            result.Result = _mapper.Map<CreateUserCommandHandlerResult>(user);
            result.StatusCode = 200;

            return result;
        }
    }

    public class CreateUserCommandHandlerResult
    {
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }
        public Guid[] RolesId { get; set; }
    }
}
