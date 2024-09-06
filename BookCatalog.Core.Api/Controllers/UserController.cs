using Application.DTOs.UserDTO;
using Application.Extensions;
using Application.Repositories;
using Application.UseCases.Authors.Command;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Core.Api.Controllers
{
    [Route("api/[controller]")]
/*    [Authorize]*/
    public class UserController : ApiControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;

        public UserController(
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IMediator mediator)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserById([FromQuery] Guid id)
        {
            User user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound($"User Id: {id} not found");

            return Ok(user);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsersAll()
        {
            IQueryable<User> Users = await _userRepository.GetAsync(x => true);

            return Ok(Users);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateUser([FromBody] CreateAuthorCommand createAuthorCommand)
        {
            var result =  await _mediator.Send(createAuthorCommand);
            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
           /* User user = _mapper.Map<User>(userCreateDTO);
            List<Role> permissions = new();
            if (user.Roles is not null)
            {
                for (int i = 0; i < user.Roles.Count; i++)
                {
                    Role permission = user.Roles.ToArray()[i];

                    permission = await _roleRepository.GetByIdAsync(permission.RoleId);

                    if (permission is null)
                    {
                        return NotFound($"Role not found");
                    }
                    permissions.Add(permission);
                }
            }
            user.Roles = permissions;
            user = await _userRepository.AddAsync(user);

            if (user is null) return BadRequest(ModelState);

            UserGetDTO userGetDTO = _mapper.Map<UserGetDTO>(user);

            return Ok(userGetDTO);*/
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO userUpdateDTO)
        {
            User user = _mapper.Map<User>(userUpdateDTO);

            user = await _userRepository.UpdateAsync(user);

            if (user is null) return BadRequest(ModelState);

            UserGetDTO userGetDTO = _mapper.Map<UserGetDTO>(user);

            return Ok(userGetDTO);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteUser([FromQuery] Guid id)
        {
            bool isDelete = await _userRepository.DeleteAsync(id);
            return isDelete ? Ok("User deleted successfully!")
                : BadRequest("Delete operation is failed");
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> ChangeUserPassword(UserChangePasswordDTO userChangePasswordDTO)
        {

            var user = await _userRepository.GetByIdAsync(userChangePasswordDTO.UserId);

            if (user is not null)
            {
                string newPassword = userChangePasswordDTO.CurrentPassword.GetHash();
                if (newPassword == user.Password
                    && userChangePasswordDTO.NewPassword == userChangePasswordDTO.ConfirmNewPassword)
                {
                    user.Password = userChangePasswordDTO.NewPassword.GetHash();
                    await _userRepository.UpdateAsync(user);

                    return Ok("Password is changed successfully✅");
                }
                else return BadRequest("Incorrect password❌");
            }

            return BadRequest("User is not found");
        }

    }
}
