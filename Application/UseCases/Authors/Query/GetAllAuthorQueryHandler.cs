using Application.DTOs.AuthorDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Authors.Query
{
    public class GetAllAuthorQuery : IRequest<IActionResult>
    { }
    public class GetAllAuthorQueryHandler : IRequestHandler<GetAllAuthorQuery, IActionResult>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public GetAllAuthorQueryHandler(
            IAuthorRepository authorRepository,
            IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetAllAuthorQuery request, CancellationToken cancellationToken)
        {
            Task<IQueryable<Author>> authors = _authorRepository.GetAsync(x => true);

            IEnumerable<AuthorGetDTO> resultAuthors = _mapper
            .Map<IEnumerable<AuthorGetDTO>>(authors.Result.AsEnumerable());

            return new OkObjectResult(resultAuthors);
        }
    }
}
