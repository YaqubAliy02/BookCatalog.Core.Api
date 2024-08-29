using Application.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Permissions.Query
{
    public class GetAllPermissionQuery : IRequest<IActionResult> { }
    public class GetAllPermissionQueryHandler : IRequestHandler<GetAllPermissionQuery, IActionResult>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetAllPermissionQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IActionResult> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Permission> permissions = await _permissionRepository.GetAsync(x => true);

            return new OkObjectResult(permissions); 
        }
    }
}
