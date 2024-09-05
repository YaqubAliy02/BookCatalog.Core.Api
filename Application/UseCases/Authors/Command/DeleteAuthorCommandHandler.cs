using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Authors.Command
{
    public class DeleteAuthorCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, IActionResult>
    {
        private readonly IAuthorRepository _authorRepository;

        public DeleteAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<IActionResult> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            bool isDelete = await _authorRepository.DeleteAsync(request.Id);

            if (isDelete) return new OkObjectResult("Author is deleted successfully");

            return new BadRequestObjectResult("Delete operation has been failed");
        }
    }
}
