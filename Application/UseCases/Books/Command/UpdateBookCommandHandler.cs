using Application.DTOs.BookDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Books.Command
{
    public class UpdateBookCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public DateTime PublishedDate { get; set; }
        public Guid[] AuthorsId { get; set; }
        public BookCategories Categories { get; set; }
    }
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, IActionResult> 
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;
        private readonly IValidator<Book> _validator;

        public UpdateBookCommandHandler(IMapper mapper, 
            IBookRepository bookRepository,
            IValidator<Book> bookValidator)
        {
            _mapper = mapper;
            _bookRepository = bookRepository;
            _validator = bookValidator;
        }

        public async Task<IActionResult> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            Book book = _mapper.Map<Book>(request);
            var validationRes = _validator.Validate(book);
            if (validationRes.IsValid)
            {
                /* for (int i = 0; i < book.Authors.Count; i++)
                 {
                     Author author = book.Authors.ToArray()[i];
                     author = await _authorRepository.GetByIdAsync(author.Id);

                     if (author is null)
                     {
                         return NotFound("Author Id: " + author.Id + "Not found ");
                     }
                 }*/
                if (book is null) return new NotFoundObjectResult("Book is not found!!!");

                book = await _bookRepository.UpdateAsync(book);

                return new OkObjectResult(_mapper.Map<BookGetDto>(book));
            }
            return new BadRequestObjectResult(validationRes);
        }
    }
}
