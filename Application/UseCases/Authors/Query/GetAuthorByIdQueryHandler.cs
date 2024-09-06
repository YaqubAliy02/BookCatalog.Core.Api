using Application.DTOs.AuthorDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Authors.Query
{
    public class GetAuthorByIdQuery : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, IActionResult>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public GetAuthorByIdQueryHandler(IAuthorRepository authorRepository,
            IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
           
            Author author = await _authorRepository.GetByIdAsync(request.Id);

            if (author is null)
            {
                return new NotFoundObjectResult($"Author Id: {request.Id} is not found");
            }

            AuthorGetDTO authorGetDTO = _mapper.Map<AuthorGetDTO>(author);

            return new OkObjectResult(authorGetDTO);
        }
    }
}
