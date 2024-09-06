using Application.DTOs.UserDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Accounts.Query
{
    public class GetAllUserQuery : IRequest<IActionResult>
    {

    }
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public GetAllUserQueryHandler(
            IMapper mapper,
            IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            Task<IQueryable<User>> Users = _userRepository.GetAsync(x => true);

            IEnumerable<UserGetDTO> resultBooks = _mapper
               .Map<IEnumerable<UserGetDTO>>(Users.Result.AsEnumerable());

            return new OkObjectResult(Users);
        }
    }
}
