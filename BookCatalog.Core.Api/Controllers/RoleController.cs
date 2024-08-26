using Application.DTOs.RoleDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public RoleController(IRoleRepository roleRepository,
            IMapper mapper, 
            IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _permissionRepository = permissionRepository;
        }

        [HttpGet("[action]")]
        public  async Task<IActionResult> GetRoleById([FromQuery] Guid id)
        {
            Role role = await _roleRepository.GetByIdAsync(id);
            
            if (role == null) 
                return NotFound($"Role Id: {id} is not found");

            return Ok(role);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllRoles()
        {
            IQueryable<Role> roles = await _roleRepository.GetAsync(x => true);

            return Ok(roles);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateDTO createRoleDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            Role role = _mapper.Map<Role>(createRoleDTO);
            List<Permission> permissions  = new();
            for(int i = 0; i < role.Permissions.Count; i++)
            {
                Permission permission = role.Permissions.ToArray()[i];
                permission = await _permissionRepository.GetByIdAsync(permission.PermissionId);
                if(permission is null)
                {
                    return NotFound($"Permission id: {permission.PermissionId} is not found");
                }

                permissions.Add(permission);
            }
            role.Permissions = permissions;
            role  = await _roleRepository.AddAsync(role);

            if (role is null)
                BadRequest(ModelState);

            RoleGetDTO roleGetDTO = _mapper.Map<RoleGetDTO>(role);

            return Ok(roleGetDTO);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleUpdateDTO roleUpdateDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            Role role = _mapper.Map<Role>(roleUpdateDTO);

            role = await _roleRepository.UpdateAsync(role);

            if(role is null)
                return BadRequest(ModelState);

            RoleGetDTO roleGetDTO = _mapper.Map<RoleGetDTO>(role);

            return Ok(roleGetDTO);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteRole([FromQuery] Guid id)
        {
            bool isDelete = await _roleRepository.DeleteAsync(id);
            return isDelete ? Ok("Role is deleted successfully") 
                : BadRequest("Deleted operation failed");
        }
    }
}
