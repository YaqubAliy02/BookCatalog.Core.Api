using Application.DTOs.BookDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Books.Query
{
    public class GetAllBookQuery : IRequest<IActionResult>
    {

    }

    public class GetAllBookQueryHandler : IRequestHandler<GetAllBookQuery, IActionResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public GetAllBookQueryHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetAllBookQuery request, CancellationToken cancellationToken)
        {

            Task<IQueryable<Book>> Books = _bookRepository.GetAsync(x => true);

            IEnumerable<BookGetDto> resultBooks = _mapper
                .Map<IEnumerable<BookGetDto>>(Books.Result.AsEnumerable());

            return new OkObjectResult(resultBooks);
        }
    }
}
