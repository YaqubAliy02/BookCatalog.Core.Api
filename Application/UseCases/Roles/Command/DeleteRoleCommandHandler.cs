
using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Roles.Command
{
    public class DeleteRoleCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, IActionResult>
    {
        private readonly IRoleRepository _roleRepository;

        public DeleteRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IActionResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            bool isDelete = await _roleRepository.DeleteAsync(request.Id);
            return isDelete ? new OkObjectResult("Role is deleted successfully")
                : new BadRequestObjectResult("Deleted operation failed");
        }
    }
}
