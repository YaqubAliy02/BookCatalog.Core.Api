using System.Text.Json;
using System.Text.Json.Serialization;
using Application.DTOs.BookDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IAppCache _lazyCache;
        private readonly IDistributedCache _distributedCache;

        private readonly string _Key = "MyLazyCache";
        public BookController(
            IBookRepository bookRepository,
            IValidator<Book> validator,
            IMapper mapper,
            IAuthorRepository authorRepository,
            IAppCache lazyCache,
            IDistributedCache distributedCache)
        {
            _bookRepository = bookRepository;
            _validator = validator;
            _mapper = mapper;
            _authorRepository = authorRepository;
            _lazyCache = lazyCache;
            _distributedCache = distributedCache;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllBooks()
        {
            /* bool isActive = _lazyCache.TryGetValue(_Key, out IEnumerable<BookGetDto> cachedBooks);

             if (!isActive)
             {
                 var books = await _bookRepository.GetAsync(x => true);
                 if (books is not null)
                 {
                     IEnumerable<BookGetDto> resultAuthors = _mapper.Map<IEnumerable<BookGetDto>>(books);

                     var entryOptions = new MemoryCacheEntryOptions()
                         .SetAbsoluteExpiration(TimeSpan.FromSeconds(30))
                         .SetSlidingExpiration(TimeSpan.FromSeconds(30));

                     _lazyCache.Add(_Key, resultAuthors, entryOptions);

                     return Ok(resultAuthors);
                 }
                 return NoContent();
             }
             return Ok(cachedBooks);*/

            /*  IEnumerable<BookGetDto> result = await _lazyCache.GetOrAdd(_Key, async options =>
              {
                  options.SetAbsoluteExpiration(TimeSpan.FromSeconds(30));
                  options.SetSlidingExpiration(TimeSpan.FromSeconds(30));
                  var books = await _bookRepository.GetAsync(x => true);

                  IEnumerable<BookGetDto> booksResult = _mapper.Map<IEnumerable<BookGetDto>>(books);

                  return booksResult;
              });

              return Ok(result);*/ //In-Memory

            string CachedBooks = await _distributedCache.GetStringAsync(_Key); //Destributed Cache

            if(string.IsNullOrEmpty(CachedBooks))
            {
                Task<IQueryable<Book>> Books = _bookRepository.GetAsync(x => true);

                IEnumerable<BookGetDto> resultBooks = _mapper
                    .Map<IEnumerable<BookGetDto>>(Books.Result.AsEnumerable());

                await _distributedCache.SetStringAsync(_Key, JsonSerializer.Serialize(resultBooks), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
                return Ok(resultBooks);
            }

            var result = JsonSerializer.Deserialize<IEnumerable<BookGetDto>>(CachedBooks);

            return Ok(result);
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
