using System.ComponentModel.DataAnnotations;
using Application.DTOs.UserDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Accounts.Command
{
    public class UpdateUsersCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public Guid[] RolesId { get; set; }
    }
    public class UpdateUsersCommandHandler : IRequestHandler<UpdateUsersCommand, IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUsersCommandHandler(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(UpdateUsersCommand request, CancellationToken cancellationToken)
        {
            User user = _mapper.Map<User>(request);
            user = await _userRepository.UpdateAsync(user);
            if (user is null) return new BadRequestObjectResult(request);
            UserGetDTO userGetDTO = _mapper.Map<UserGetDTO>(user);

            return new OkObjectResult(userGetDTO);
        }
    }
}
