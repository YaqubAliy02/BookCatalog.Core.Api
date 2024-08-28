using Application.DTOs.PermissionDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class PermissionController : ApiControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;
        public PermissionController(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetPermissionById([FromQuery] Guid id)
        {
            Permission permission = await _permissionRepository.GetByIdAsync(id);
            
            if(permission is null)
            {
                return NotFound($"Permission Id:{id} is not found");
            }
            return Ok(permission);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPermission()
        {
           IQueryable<Permission> permissions = await _permissionRepository.GetAsync(x => true);

            return Ok(permissions);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionDTO createPermissionDTO)
        {
            Permission permission =  _mapper.Map<Permission>(createPermissionDTO);
            permission = await _permissionRepository.AddAsync(permission);

            if (permission is null) return BadRequest(ModelState);

            return Ok(createPermissionDTO);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdatePermission([FromBody] Permission UpdatePermissionDTO)
        {
            Permission permission = _mapper.Map<Permission>(UpdatePermissionDTO);
            permission = await _permissionRepository.UpdateAsync(permission);

            if (permission is null) 
                return NotFound();

            return Ok(permission);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeletePermission([FromQuery] Guid id)
        {
            bool isDelete = await _permissionRepository.DeleteAsync(id);

            return isDelete ? Ok("Deleted successfully") : BadRequest("Delete operation failed");
        }


    }
}
