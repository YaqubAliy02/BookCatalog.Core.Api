
using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Permissions.Commands
{
    public class DeletePermissionQuery : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }

    public class DeletePermissionQueryHandler : IRequestHandler<DeletePermissionQuery, IActionResult>
    {
        private readonly IPermissionRepository _permissionRepository;

        public DeletePermissionQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IActionResult> Handle(DeletePermissionQuery request, CancellationToken cancellationToken)
        {
            bool isDelete = await _permissionRepository.DeleteAsync(request.Id);

            return isDelete ? new OkObjectResult("Deleted successfully") :
                new BadRequestObjectResult("Delete operation failed");
        }
    }
}
