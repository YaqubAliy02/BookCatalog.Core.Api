
using Application.Abstraction;
using Application.DTOs.UserDTO;
using Application.Extensions;
using Application.Models;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
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
                Token token = new()
                {
                    AccessToken = _tokenService.CreateToken(user)
                };

                RegisteredUserDTO userDTO = new()
                {
                    User = user,
                    UserTokens = token
                };

                return Ok(userDTO);
            }
            return BadRequest("Email or Password is incorrect!!!");
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
                    Token token = new()
                    {
                        AccessToken = _tokenService.CreateToken(user)
                    };

                    RegisteredUserDTO userDTO = new()
                    {
                        User = user,
                        UserTokens = token
                    };  

                    userDTO.UserTokens = token;

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
