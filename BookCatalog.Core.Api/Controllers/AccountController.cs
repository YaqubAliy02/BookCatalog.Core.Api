using Application.Abstraction;
using Application.Repositories;
using Application.UseCases.Accounts.Command;
using Application.UseCases.Accounts.Query;
using BookCatalog.Core.Api.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Core.Api.Controllers
{
    [Route("api/[controller]")]

    public class AccountController : ApiControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMediator _mediator;
        public AccountController(ITokenService tokenService,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IMediator mediator)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand loginUserCommand)
        {
            return await _mediator.Send(loginUserCommand);
            /* var user = (await _userRepository.GetAsync(x =>
                 x.Password == userCredentials.Password.GetHash() &&
                 x.Email == userCredentials.Email)).FirstOrDefault();

             if (user is not null)
             {

                 RegisteredUserDTO userDTO = new()
                 {
                     User = user,
                     UserTokens = await _tokenService.CreateTokensAsync(user)
                 };

                 return Ok(userDTO);
             }
             return BadRequest("Email or Password is incorrect!!!");*/
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand token)
        {
            var result = await _mediator.Send(token);

            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
            /* var principal = _tokenService.GetClaimsFromExpiredToken(token.AccessToken);
             string email = principal.FindFirstValue(ClaimTypes.Email);

             if (email is null)
             {
                 return NotFound("Refresh token is not found");
             }
             RefreshToken savedRefreshToken = _tokenService.Get(x =>
                 x.Email == email && x.RefreshTokenValue == token.RefereshToken).AsNoTracking().FirstOrDefault();

             if (savedRefreshToken is null)
             {
                 return BadRequest("Refresh token or access token is invalid");
             }
             if (savedRefreshToken.ExpiredDate < DateTime.UtcNow)
             {
                 _tokenService.Delete(savedRefreshToken);
                 return StatusCode(405, "Refresh token already expired please login again");
             }
             Token newTokens = await _tokenService
                 .CreateTokenFromRefresh(principal, savedRefreshToken);

             return Ok(newTokens);*/
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Create([FromBody] RegisterUserCommand newUser)
        {
            var result = await _mediator.Send(newUser);

            return result.StatusCode == 200 ? Ok(newUser) : BadRequest(result);
            /*  User user = _mapper.Map<User>(newUser);
              List<Role> permissions = new();

              if (user.Roles is not null)
              {
                  for (int i = 0; i < user.Roles.Count; i++)
                  {
                      Role permission = user.Roles.ToArray()[i];

                      permission = await _roleRepository.GetByIdAsync(permission.RoleId);

                      if (permission is null)
                      {
                          return NotFound("Role is not found");
                      }
                      permissions.Add(permission);
                  }
              }

              user.Roles = permissions;
              user = await _userRepository.AddAsync(user);

              if (newUser is not null)
              {
                  RegisteredUserDTO userDTO = new()
                  {
                      User = user,
                      UserTokens = await _tokenService.CreateTokensAsync(user)
                  };

                  return Ok(userDTO);
              }

              return BadRequest(ModelState);*/
        }

        [HttpGet("[action]")]
        [CustomAuthorizationFilter("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            return await _mediator.Send(new GetAllUserQuery());
            /* IQueryable<User> result = await _userRepository.GetAsync(x => true);

             return Ok(result);*/
        }

        [HttpPut("[action]")]
        [CustomAuthorizationFilter("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUsersCommand updateUserCommand)
        {
            return await _mediator.Send(updateUserCommand);
        }
    }
}
