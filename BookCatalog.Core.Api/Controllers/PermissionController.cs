using Application.UseCases.Permissions.Commands;
using Application.UseCases.Permissions.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetPermissionById([FromQuery] GetPermissionByIdQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPermission()
        {
            return await _mediator.Send(new GetAllPermissionQuery());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionCommand createPermissionCommand)
        {
            return await _mediator.Send(createPermissionCommand);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdatePermission([FromBody] UpdatePermissionQuery updatePermissionQuery)
        {
            return await _mediator.Send(updatePermissionQuery);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeletePermission([FromQuery] DeletePermissionQuery deletePermissionQuery)
        {
            return await _mediator.Send(deletePermissionQuery);
        }
    }
}
