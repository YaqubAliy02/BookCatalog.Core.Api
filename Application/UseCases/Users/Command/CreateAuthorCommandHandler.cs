using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.UserDTO;
using Application.Repositories;
using Domain.Entities;
using AutoMapper;

namespace Application.UseCases.Users.Command
{
    public class CreateAuthorCommand : IRequest<IActionResult> 
    {
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }
        public Guid[] RolesId { get; set; }
    }
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public CreateAuthorCommandHandler(
            IMapper mapper, 
            IRoleRepository roleRepository, 
            IUserRepository userRepository)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            User user = _mapper.Map<User>(request);
            List<Role> permissions = new();
            if (user.Roles is not null)
            {
                for (int i = 0; i < user.Roles.Count; i++)
                {
                    Role permission = user.Roles.ToArray()[i];

                    permission = await _roleRepository.GetByIdAsync(permission.RoleId);

                    if (permission is null)
                    {
                        return new NotFoundObjectResult($"Role not found");
                    }
                    permissions.Add(permission);
                }
            }
            user.Roles = permissions;
            user = await _userRepository.AddAsync(user);

            if (user is null) return new BadRequestObjectResult(user);

            UserGetDTO userGetDTO = _mapper.Map<UserGetDTO>(user);

            return new OkObjectResult(userGetDTO);
        }
    }

    public class CreateAuthorCommandHandlerResult
    {
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }
        public Guid[] RolesId { get; set; }
    }
}
