using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Books.Command
{
    public class SearchBookCommand : IRequest<IActionResult>
    {
        public string SearchText { get; set; }
    }
    public class SearchBookCommandHandler : IRequestHandler<SearchBookCommand, IActionResult>
    {
        private readonly IBookRepository _bookRepository;

        public SearchBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IActionResult> Handle(SearchBookCommand request, CancellationToken cancellationToken)
        {
            return new OkObjectResult(_bookRepository.SearchBook(request.SearchText).AsEnumerable());
        }
    }
}
