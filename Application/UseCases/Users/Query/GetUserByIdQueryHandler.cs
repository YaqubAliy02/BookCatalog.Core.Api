using Application.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Users.Query
{
    public class GetUserByIdQuery : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, IActionResult>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            User user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null) return new NotFoundObjectResult($"User Id: {request.Id} not found");

            return new OkObjectResult(user);
        }
    }
}
