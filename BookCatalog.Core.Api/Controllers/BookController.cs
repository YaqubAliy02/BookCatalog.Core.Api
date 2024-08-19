using Application.DTOs.BookDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace BookCatalog.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IValidator<Book> _validator;
        private readonly IMapper _mapper;
        public BookController(IBookRepository bookRepository,
            IValidator<Book> validator, IMapper mapper,
            IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _validator = validator;
            _mapper = mapper;
            _authorRepository = authorRepository;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookRepository.GetAsync(x => true);
            var resultAuthors = _mapper.Map<IEnumerable<Book>>(books);

            return Ok(resultAuthors);
        }

        [HttpGet("[action]/{id}")]

        public async Task<IActionResult> GetBookByIdAsync(Guid id)
        {
            Book book = await _bookRepository.GetByIdAsync(id);

            if (book is null) return NotFound($"Book id: {id} is not found!!!");

            return Ok(_mapper.Map<BookGetDto>(book));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateBookAsync([FromBody] BookCreateDto bookCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Book book = _mapper.Map<Book>(bookCreate);
            var validationRes = _validator.Validate(book);

            if (!validationRes.IsValid)
                return BadRequest(validationRes);

            if (book is null) return NotFound("Book is not found!!!");

            for (int i = 0; i < book.Authors.Count; i++)
            {
                Author author = book.Authors.ToArray()[i];
                author = await _authorRepository.GetByIdAsync(author.Id);

                if (author is null)
                {
                    return NotFound("Author Id: " + author.Id + "Not found ");
                }
            }
            book = await _bookRepository.AddAsync(book);
            return Ok(_mapper.Map<BookGetDto>(book));

        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateBookAsync([FromBody] BookUpdateDTO bookUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Book book = _mapper.Map<Book>(bookUpdate);
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
                book = await _bookRepository.UpdateAsync(book);

                if (book is null) return NotFound("Book is not found!!!");

                return Ok(_mapper.Map<BookGetDto>(book));
            }
            return BadRequest(validationRes);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteBook([FromQuery] Guid bookId)
        {
            bool maybeDelete = await _bookRepository.DeleteAsync(bookId);

            if (maybeDelete) return Ok("Deleted successfully");

            return BadRequest("Deleting operation has been failed!!!");
        }
    }
}
