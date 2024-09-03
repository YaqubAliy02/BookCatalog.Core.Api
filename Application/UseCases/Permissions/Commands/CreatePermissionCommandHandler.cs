using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Permissions.Commands
{
    public class CreatePermissionCommand : IRequest<IActionResult>
    {
        [Required]
        public string PermissionName { get; set; }
    }
    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, IActionResult>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        public CreatePermissionCommandHandler(IPermissionRepository permissionRepository,
            IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(
            CreatePermissionCommand request,
            CancellationToken cancellationToken)
        {
            Permission permission = _mapper.Map<Permission>(request);
            permission = await _permissionRepository.AddAsync(permission);

            if (permission is null) return new BadRequestObjectResult("Permission is not found");

            return new OkObjectResult(permission);
        }
    }
}
