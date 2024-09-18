using Application.DTOs.BookDTO;
using Application.Repositories;
using Application.UseCases.Books.Command;
using Application.UseCases.Books.Query;
using BookCatalog.Core.Api.Filters;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class BookController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IBookRepository _bookRepository;
        public BookController(IMediator mediator, IBookRepository bookRepository)
        {
            _mediator = mediator;
            _bookRepository = bookRepository;
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllBooks()
        {
            return await _mediator.Send(new GetAllBookQuery());
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

            /*           string CachedBooks = await _distributedCache.GetStringAsync(_Key); //Destributed Cache

           // if (string.IsNullOrEmpty(CachedBooks))
           // {
                Task<IQueryable<Book>> Books = _bookRepository.GetAsync(x => true);

                IEnumerable<BookGetDto> resultBooks = _mapper
                    .Map<IEnumerable<BookGetDto>>(Books.Result.AsEnumerable());

                await _distributedCache.SetStringAsync(_Key, JsonSerializer.Serialize(resultBooks), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
                });/*
                return Ok(resultBooks);
            //}

           // var result = JsonSerializer.Deserialize<IEnumerable<BookGetDto>>(CachedBooks);

           // return Ok(result);*/
        }

        [HttpGet("[action]/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBookByIdAsync(Guid id)
        {
            var getBookByIdQuery = new GetBookByIdQuery { Id = id };
            return await _mediator.Send(getBookByIdQuery);
        }

        [HttpPost("[action]")]
        /* [CustomAuthorizationFilter("CreateBook")]*/
        [AllowAnonymous]
        public async Task<IActionResult> CreateBookAsync([FromBody] CreateBookCommand bookCreate)
        {
            var result = await _mediator.Send(bookCreate);

            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }

        #region CreateBookWithoutMediatr
        /*   [HttpPost("[action]")]
           [CustomAuthorizationFilter("CreateBook")]
           public async Task<IActionResult> CreateBookAsync([FromBody] BookCreateDto bookCreate)
           {
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

           }*/
        #endregion

        [HttpPut("[action]")]
        [CustomAuthorizationFilter("UpdateBook")]
        public async Task<IActionResult> UpdateBookAsync([FromBody] UpdateBookCommand updateBookCommand)
        {
            return await _mediator.Send(updateBookCommand);
        }

        [HttpDelete("[action]")]
        /* [CustomAuthorizationFilter("DeleteBook")]*/
        public async Task<IActionResult> DeleteBook([FromQuery] DeleteBookCommand deleteBookCommand)
        {
            return await _mediator.Send(deleteBookCommand);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SearchBook([FromQuery] SearchBookCommand searchBookCommand)
        {
            return await _mediator.Send(searchBookCommand);
        }

        [HttpGet("[action]")]
        public IActionResult GetBookCategories()
        {
            var categories = Enum.GetValues(typeof(BookCategories))
                                 .Cast<BookCategories>()
                                 .Select(c => new { Id = (byte)c, Name = c.ToString() })
                                 .ToList();

            return Ok(categories);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> FilterBooks([FromQuery] Guid? authorId = null, [FromQuery] BookCategories? category = null)
        {
            try
            {
                IQueryable<Book> booksQuery = await _bookRepository.GetAsync(x => true);

                if (category.HasValue)
                {
                    booksQuery = booksQuery.Where(b => b.Categories == category.Value);
                }

                if (authorId.HasValue)
                {
                    booksQuery = booksQuery.Where(b => b.Authors.Any(a => a.Id == authorId.Value));
                }

                var books = booksQuery.ToList();

                IEnumerable<BookGetDto> resultBooks = _mapper.Map<IEnumerable<BookGetDto>>(books);

                return Ok(resultBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

    }
}
