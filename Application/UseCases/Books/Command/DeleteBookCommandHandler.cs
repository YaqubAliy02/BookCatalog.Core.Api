using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Books.Command
{
    public class DeleteBookCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, IActionResult>
    {
        private readonly IBookRepository _bookRepository;

        public DeleteBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IActionResult> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            bool maybeDelete = await _bookRepository.DeleteAsync(request.Id);

            if (maybeDelete) return new OkObjectResult("Deleted successfully");

            return new BadRequestObjectResult("Deleting operation has been failed!!!");
        }
    }
}
