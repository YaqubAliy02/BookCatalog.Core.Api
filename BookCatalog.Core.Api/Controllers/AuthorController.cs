using Application.DTOs.AuthorDTO;
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
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IValidator<Author> _validator;
        private readonly IMapper _mapper;

        public AuthorController(IAuthorRepository authorRepository,
            IBookRepository bookRepository,
            IValidator<Author> validator,
            IMapper mapper)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _validator = validator;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAuthorById([FromQuery] Guid id)
        {
            Author author = await _authorRepository.GetByIdAsync(id);

            if (author is null)
            {
                return NotFound($"Author Id: {id} is not found");
            }

            AuthorGetDTO authorGetDTO = _mapper.Map<AuthorGetDTO>(author);
                return Ok(authorGetDTO);



        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _authorRepository.GetAsync(x => true);
            var resultAuthors = _mapper.Map<IEnumerable<Author>>(authors);

            return Ok(resultAuthors);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorCreateDTO createDTO)
        {
            if (ModelState.IsValid)
            {
                Author author = _mapper.Map<Author>(createDTO);
                var validResult = _validator.Validate(author);

                if (validResult.IsValid)
                {
                  /*  for (int i = 0; i < author.Books.Count; i++)
                    {
                        Book book = author.Books.ToArray()[i];

                        book = await _bookRepository.GetByIdAsync(book.Id);

                        if (book is null)
                        {
                            return NotFound($"Book id: {book.Id} is not found");
                        }
                    }*/
                    author = await _authorRepository.AddAsync(author);

                    if (author is null) return NotFound();

                    AuthorGetDTO authorGetDTO = _mapper.Map<AuthorGetDTO>(author);

                    return Ok(authorGetDTO);
                }
                return BadRequest(validResult);
            }

            return BadRequest(ModelState);
        }


        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateBookAsync([FromBody] AuthorUpdateDTO authorUpdateDO)
        {
            if (ModelState.IsValid)
            {
                Author author = _mapper.Map<Author>(authorUpdateDO);
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

                    if (author is null) return NotFound();

                    author = await _authorRepository.UpdateAsync(author);
                    AuthorGetDTO authorGetDTO = _mapper.Map<AuthorGetDTO>(author);

                    return Ok(authorGetDTO);
                }
                return BadRequest(validationRes);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteAuthor([FromQuery] Guid id)
        {
            bool isDelete = await _authorRepository.DeleteAsync(id);

            if (isDelete) return Ok("Author is deleted successfully");

            return BadRequest("Delete operation has been failed");
        }
    }
}
