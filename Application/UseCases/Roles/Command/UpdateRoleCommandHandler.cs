using Application.DTOs.RoleDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Roles.Command
{
    public class UpdateRoleCommand : IRequest<IActionResult>
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public Guid[] PermissionsId { get; set; }
    }
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, IActionResult>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public UpdateRoleCommandHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            Role role = _mapper.Map<Role>(request);

            role = await _roleRepository.UpdateAsync(role);

            if (role is null)
                return new NotFoundObjectResult("Role is not found to complete Update operation!!!");

            RoleGetDTO roleGetDTO = _mapper.Map<RoleGetDTO>(role);

            return new  OkObjectResult(roleGetDTO);
        }
    }
}
