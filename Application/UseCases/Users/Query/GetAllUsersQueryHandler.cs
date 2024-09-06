using Application.DTOs.BookDTO;
using Application.DTOs.UserDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Users.Query
{
    public class GetAllUsersQuery : IRequest<IActionResult> { }
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            Task<IQueryable<User>> Users =  _userRepository.GetAsync(x => true);

            IEnumerable<UserGetDTO> resultBooks = _mapper
               .Map<IEnumerable<UserGetDTO>>(Users.Result.AsEnumerable());

            return new OkObjectResult(Users);
        }
    }
}
