using Application.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Roles.Query
{
    public class GetRoleByIdQuery : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }

    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, IActionResult>
    {
        private readonly IRoleRepository _roleRepository;

        public GetRoleByIdQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IActionResult> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            Role role = await _roleRepository.GetByIdAsync(request.Id);

            if (role == null)
                return new NotFoundObjectResult($"Role Id: {request.Id} is not found");

            return new OkObjectResult(role);
        }
    }
}
