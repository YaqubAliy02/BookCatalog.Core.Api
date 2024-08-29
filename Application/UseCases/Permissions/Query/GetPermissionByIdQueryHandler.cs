using Application.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Permissions.Query
{
    public class GetPermissionByIdQuery : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, IActionResult>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetPermissionByIdQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        async Task<IActionResult> IRequestHandler<GetPermissionByIdQuery, IActionResult>.Handle(
           GetPermissionByIdQuery request,
           CancellationToken cancellationToken)
        {
            Permission permission = await _permissionRepository.GetByIdAsync(request.Id);

            if (permission is null)
            {
                return new NotFoundObjectResult($"Permission Id:{request.Id} is not found");
            }
            return new OkObjectResult(permission);
        }
    }
}
