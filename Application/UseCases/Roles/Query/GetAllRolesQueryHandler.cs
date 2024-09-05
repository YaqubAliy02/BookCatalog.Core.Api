using Application.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Roles.Query
{
    public class GetAllRolesQuery : IRequest<IActionResult> { }
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IActionResult>
    {
        private readonly IRoleRepository _roleRepository;

        public GetAllRolesQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IActionResult> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Role> roles = await _roleRepository.GetAsync(x => true);

            return new OkObjectResult(roles);
        }
    }
}
