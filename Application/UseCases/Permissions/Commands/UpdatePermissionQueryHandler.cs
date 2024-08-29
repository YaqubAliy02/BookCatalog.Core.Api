
using System.ComponentModel.DataAnnotations.Schema;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Permissions.Commands
{
    public class UpdatePermissionQuery : IRequest<IActionResult>
    {
        public Guid PermissionId { get; set; }
        [Column("permission_name")]
        public string PermissionName { get; set; }
    }
    public class UpdatePermissionQueryHandler : IRequestHandler<UpdatePermissionQuery, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly IPermissionRepository _permissionRepository;

        public UpdatePermissionQueryHandler(IMapper mapper,
            IPermissionRepository permissionRepository)
        {
            _mapper = mapper;
            _permissionRepository = permissionRepository;
        }

        public async Task<IActionResult> Handle(UpdatePermissionQuery request, CancellationToken cancellationToken)
        {
            Permission permission = _mapper.Map<Permission>(request);
            permission = await _permissionRepository.UpdateAsync(permission);

            if (permission is null)
                return new NotFoundObjectResult("Permission is not found to complete Delete operation!!!");

            return new OkObjectResult(permission);
        }
    }
}
