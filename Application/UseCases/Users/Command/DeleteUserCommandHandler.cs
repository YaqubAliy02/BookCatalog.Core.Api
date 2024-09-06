using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Users.Command
{
    public class DeleteUserCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, IActionResult>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            bool isDelete = await _userRepository.DeleteAsync(request.Id);
            return isDelete ? new OkObjectResult("User deleted successfully!")
                : new BadRequestObjectResult("Delete operation is failed");
        }
    }
}
