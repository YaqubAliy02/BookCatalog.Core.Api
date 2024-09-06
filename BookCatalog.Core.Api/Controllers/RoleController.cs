using Application.DTOs.RoleDTO;
using Application.Repositories;
using Application.UseCases.Roles.Command;
using Application.UseCases.Roles.Query;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : ApiControllerBase
    {
        private IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetRoleById([FromQuery] GetRoleByIdQuery getRoleByIdQuery)
        {
            return await _mediator.Send(getRoleByIdQuery);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllRoles()
        {
           return await _mediator.Send(new GetAllRolesQuery());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand createRoleCommand)
        {
            var result = await _mediator.Send(createRoleCommand);
           
            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleCommand updateRoleCommand )
        {
            return await _mediator.Send(updateRoleCommand);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteRole([FromQuery] DeleteRoleCommand deleteRoleCommand)
        {
            return await _mediator.Send(deleteRoleCommand);
        }
    }
}
