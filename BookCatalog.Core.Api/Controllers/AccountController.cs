
using Application.Abstraction;
using Application.DTOs.UserDTO;
using Application.Extensions;
using Application.Models;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public AccountController(ITokenService tokenService,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromForm] UserCredentials userCredentials)
        {
            var user = (await _userRepository.GetAsync(x =>
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
            return BadRequest("Email or Password is incorrect!!!");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] Token token)
        {
            var principal = _tokenService.GetClaimsFromExpiredToken(token.AccessToken);
            string email = principal.Identity.Name;
            
            if(email is null)
            {
                return NotFound("Refresh token is not found");
            }
            RefreshToken savedRefreshToken = _tokenService.Get(x =>
                x.Email == email && x.RefreshTokenValue == token.RefereshToken).FirstOrDefault();

            if (savedRefreshToken is null)
            {
                return BadRequest("Refresh token or access token is invalid");
            }
            if(savedRefreshToken.ExpiredDate < DateTime.UtcNow)
            {
                _tokenService.Delete(savedRefreshToken);
                return StatusCode(405, "Refresh token already expired please login again");
            }

            Token newTokens = await _tokenService
                .CreateTokenFromRefresh(principal,savedRefreshToken);

            return Ok(newTokens);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Create([FromBody] UserCreateDTO newUser)
        {
            if (ModelState.IsValid)
            {
                User user = _mapper.Map<User>(newUser);
                user.Password = user.Password.GetHash();
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
            }
            return BadRequest();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUsers()
        {
            IQueryable<User> result = await _userRepository.GetAsync(x => true);

            return Ok(result);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                if (await _userRepository.UpdateAsync(user) is not null)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }
    }
}
