using Application.DTOs.BookDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Books.Query
{
    public class GetBookByIdQuery: IRequest<IActionResult> 
    {
        public Guid Id { get; set; }
    }
    public class GetBookByIdQueryHandler : IRequestHandler<GetAllBookQuery, IActionResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public GetBookByIdQueryHandler(IBookRepository bookRepository, 
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetAllBookQuery request, CancellationToken cancellationToken)
        {
            Book book = await _bookRepository.GetByIdAsync(request.Id);

            if (book is null) return new NotFoundObjectResult($"Book id: {id} is not found!!!");

            return new OkObjectResult(_mapper.Map<BookGetDto>(book));
        }
    }
}
