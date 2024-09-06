using System.ComponentModel.DataAnnotations;
using Application.DTOs.UserDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Users.Command
{
    public class UpdateUserCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public Guid[] RolesId { get; set; }
    }
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(
            IMapper mapper,
            IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            User user = _mapper.Map<User>(request);

            user = await _userRepository.UpdateAsync(user);

            if (user is null) return new BadRequestObjectResult(request);

            UserGetDTO userGetDTO = _mapper.Map<UserGetDTO>(user);

            return new OkObjectResult(userGetDTO);
        }
    }
}
