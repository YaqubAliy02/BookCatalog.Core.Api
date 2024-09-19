using Application.Models;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Authors.Command
{
    public class CreateAuthorCommand : IRequest<ResponseCore<CreateAuthorCommandHandlerResult>>
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string AuthorPhoto { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
        public string AboutAuthor { get; set; }
    }

    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, ResponseCore<CreateAuthorCommandHandlerResult>>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<Author> _validator;
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;

        public CreateAuthorCommandHandler(
            IMapper mapper,
            IValidator<Author> validator,
            IAuthorRepository authorRepository,
            IBookRepository bookRepository)
        {
            _mapper = mapper;
            _validator = validator;
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }

        public async Task<ResponseCore<CreateAuthorCommandHandlerResult>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var result = new ResponseCore<CreateAuthorCommandHandlerResult>();

            Author author = _mapper.Map<Author>(request);
            var validationResult = _validator.Validate(author);

            if (!validationResult.IsValid)
            {
                result.ErrorMessage = validationResult.Errors.ToArray();
                result.StatusCode = 400;

                return result;
            }
            if (author is null)
            {
                result.ErrorMessage = new string[] { "Author is not found" };
                result.StatusCode = 400;

                return result;
            }

            if (author.Books is not null)
            {
                List<Book> books = new();
                for (int i = 0; i < author.Books.Count; i++)
                {
                    Book book = author.Books.ToArray()[i];
                    book = await _bookRepository.GetByIdAsync(book.Id);

                    if (book is null)
                    {
                        result.ErrorMessage = new string[] { "Book Id: " + book.Id + "Not found " };
                        result.StatusCode = 400;

                        return result;
                    }
                    books.Add(book);
                }
                author.Books = books;
                author = await _authorRepository.AddAsync(author);

                result.Result = _mapper.Map<CreateAuthorCommandHandlerResult>(author);
                result.StatusCode = 200;

                return result;
            }

            author = await _authorRepository.AddAsync(author);

            result.Result = _mapper.Map<CreateAuthorCommandHandlerResult>(author);
            result.StatusCode = 200;

            return result;
        }
    }

    public class CreateAuthorCommandHandlerResult
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string AuthorPhoto { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
        public string AboutAuthor { get; set; }
    }
}
