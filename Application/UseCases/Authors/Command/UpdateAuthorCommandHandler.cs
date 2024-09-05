using Application.DTOs.AuthorDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Authors.Command
{
    public class UpdateAuthorCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
    }
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<Author> _validator;
        private readonly IAuthorRepository _authorRepository;

        public UpdateAuthorCommandHandler(
            IMapper mapper,
            IValidator<Author> validator,
            IAuthorRepository authorRepository)
        {
            _mapper = mapper;
            _validator = validator;
            _authorRepository = authorRepository;
        }

        public async Task<IActionResult> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            Author author = _mapper.Map<Author>(request);
            var validationRes = _validator.Validate(author);

            if (validationRes.IsValid)
            {
                /* for (int i = 0; i < author.Books.Count; i++)
                 {
                     Book book = author.Books.ToArray()[i];
                     author = await _authorRepository.GetByIdAsync(author.Id);

                     if (author is null)
                     {
                         return NotFound("Author Id: " + author.Id + "Not found ");
                     }
                 }*/

                if (author is null) return new NotFoundObjectResult("Author is not found");

                author = await _authorRepository.UpdateAsync(author);
                AuthorGetDTO authorGetDTO = _mapper.Map<AuthorGetDTO>(author);


                return new OkObjectResult(authorGetDTO);
            }
            return new BadRequestObjectResult(validationRes);
        }
    }
}
