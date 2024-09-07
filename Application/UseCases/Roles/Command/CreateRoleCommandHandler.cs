using Application.Models;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Roles.Command
{
    public class CreateRoleCommand : IRequest<ResponseCore<CreateRoleCommandResult>>
    {
        public string RoleName { get; set; }
        public Guid[] PermissionId { get; set; }
    }
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ResponseCore<CreateRoleCommandResult>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IValidator<Role> _validator;

        public CreateRoleCommandHandler(IRoleRepository roleRepository,
            IMapper mapper,
            IPermissionRepository permissionRepository,
            IValidator<Role> validator)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _permissionRepository = permissionRepository;
            _validator = validator;
        }

        public async Task<ResponseCore<CreateRoleCommandResult>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var result = new ResponseCore<CreateRoleCommandResult>();

            Role role = _mapper.Map<Role>(request);
            var validationResult = _validator.Validate(role);

            if (!validationResult.IsValid)
            {
                result.ErrorMessage = validationResult.Errors.ToArray();
                result.StatusCode = 400;

                return result;
            }

            if (role is null)
            {
                result.ErrorMessage = new string[] { "Role is not found" };
                result.StatusCode = 400;

                return result;
            }
            List<Permission> permissions = new();

            for (int i = 0; i < role.Permissions.Count; i++)
            {
                Permission permission = role.Permissions.ToArray()[i];
                permission = await _permissionRepository.GetByIdAsync(permission.PermissionId);
                if (permission is null)
                {
                    result.ErrorMessage = new string[] { "Role Id: " + permission.PermissionId + "Not found " };
                    result.StatusCode = 400;

                    return result;
                }

                permissions.Add(permission);
            }
            role.Permissions = permissions;
            role = await _roleRepository.AddAsync(role);
            result.Result = _mapper.Map<CreateRoleCommandResult>(role);
            result.StatusCode = 200;

            return result;

        }
    }

    public class CreateRoleCommandResult
    {
        public string RoleName { get; set; }
        public Guid[] PermissionId { get; set; }
    }
}
