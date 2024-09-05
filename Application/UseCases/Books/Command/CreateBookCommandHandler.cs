using Application.DTOs.BookDTO;
using Application.Models;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Books.Command
{
    public class CreateBookCommand : IRequest<ResponseCore<CreateBookCommandResult>>
    {
        public string Name { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public DateTime PublishedDate { get; set; }
        public Guid[] AuthorsId { get; set; }
        public BookCategories Categories { get; set; }
    }
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, ResponseCore<CreateBookCommandResult>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IValidator<Book> _validator;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CreateBookCommandHandler(IBookRepository bookRepository,
            IAuthorRepository authorRepository,
            IValidator<Book> validator,
            IMediator mediator,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _validator = validator;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<ResponseCore<CreateBookCommandResult>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var result = new ResponseCore<CreateBookCommandResult>();

            Book book = _mapper.Map<Book>(request);
            var validationRes = _validator.Validate(book);

            if (!validationRes.IsValid)
            {
                result.ErrorMessage = validationRes.Errors.ToArray();
                result.StatusCode = 400;

                return result;
            }

            if (book is null)
            {
                result.ErrorMessage = new string[] { "Book is not found" };
                result.StatusCode = 400;

                return result;
            }
            List<Author> authors = new();

            for (int i = 0; i < book.Authors.Count; i++)
            {
                Author author = book.Authors.ToArray()[i];
                author = await _authorRepository.GetByIdAsync(author.Id);

                if (author is null)
                {
                    result.ErrorMessage = new string[] { "Author Id: " + author.Id + "Not found " };
                    result.StatusCode = 400;

                    return result;
                }
               authors.Add(author);
            }

            book.Authors = authors;
            book = await _bookRepository.AddAsync(book);
            result.Result = _mapper.Map<CreateBookCommandResult>(book);
            result.StatusCode = 200;

            return result;
        }
    }

    public class CreateBookCommandResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public DateTime PublishedDate { get; set; }
        public Guid[] AuthorsId { get; set; }
        public BookCategories Categories { get; set; }
    }
}
