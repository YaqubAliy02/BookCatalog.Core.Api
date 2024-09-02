using Application.DTOs.AuthorDTO;
using Application.Repositories;
using BookCatalog.Core.Api.Filters;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BookCatalog.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthorController : ApiControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IValidator<Author> _validator;
        private readonly IMemoryCache _memoryCache;

        private readonly string _Cache_Key = "Key";

        public AuthorController(IAuthorRepository authorRepository,
            IBookRepository bookRepository,
            IValidator<Author> validator,
            IMemoryCache memoryCache)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _validator = validator;
            _memoryCache = memoryCache;
        }

        [HttpGet("[action]")]
        //[CustomAuthorizationFilter("GetAuthorById")]
        public async Task<IActionResult> GetAuthorById([FromQuery] Guid id)
        {
            if (_memoryCache.TryGetValue(id.ToString(), out AuthorGetDTO cachedAuthor))
            {
                return Ok(cachedAuthor);
            }
            Author author = await _authorRepository.GetByIdAsync(id);

            if (author is null)
            {
                return NotFound($"Author Id: {id} is not found");
            }

            AuthorGetDTO authorGetDTO = _mapper.Map<AuthorGetDTO>(author);

            return Ok(authorGetDTO);
        }

        [HttpGet("[action]")]
        /*   // [ResponseCache(Duration = 20)] //response cache in controller
           //[OutputCache(Duration = 20)]// output cach in controller
           //[EnableRateLimiting("FixedWindow")] // When we use Rate Limit for specific
           // action in our Project we should add attribute for action
           // which we want to use Rate Limiters*/
     //   [CacheResourceFilter("GetAuthors")]
       // [CustomAuthorizationFilter("GetAllAuthors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            /*            bool cacheHit = _memoryCache.TryGetValue(_Cache_Key, out IEnumerable<AuthorGetDTO> cachedAuthor);
                        if (!cacheHit)
                        {

                            Console.WriteLine("CacheHit is false");
                            var options = new MemoryCacheEntryOptions()
                                .SetAbsoluteExpiration(TimeSpan.FromSeconds(30))
                                .SetSlidingExpiration(TimeSpan.FromSeconds(30));

                            IQueryable<Author> authors = await _authorRepository.GetAsync(x => true);
                            var resultAuthors = _mapper.Map<IEnumerable<AuthorGetDTO>>(authors);
                            _memoryCache.Set(_Cache_Key, resultAuthors, options);

                            return Ok(resultAuthors);
                        }*/
            IEnumerable<AuthorGetDTO> CachedAuthors = _memoryCache.GetOrCreate(_Cache_Key, option =>
            {
                option.SetAbsoluteExpiration(TimeSpan.FromSeconds(1));
                option.SetSlidingExpiration(TimeSpan.FromSeconds(1));

                Task<IQueryable<Author>> authors = _authorRepository.GetAsync(x => true);
                IEnumerable<AuthorGetDTO> resultAuthors = _mapper.Map<IEnumerable<AuthorGetDTO>>(authors.Result.AsEnumerable());

                return resultAuthors;
            });

            Console.WriteLine("GetAllAuthors return json");
            return Ok(CachedAuthors);
        }

        [HttpPost("[action]")]
    //    [CustomAuthorizationFilter("CreateAuthor")]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorCreateDTO createDTO)
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
                _memoryCache.Remove(_Cache_Key);

                return Ok(authorGetDTO);
            }
            return BadRequest(validResult);
        }

        [HttpPut("[action]")]
       // [CustomAuthorizationFilter("UpdateBook")]
        public async Task<IActionResult> UpdateBookAsync([FromBody] AuthorUpdateDTO authorUpdateDO)
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
                _memoryCache.Remove(author.Id);
                _memoryCache.Remove(_Cache_Key);

                return Ok(authorGetDTO);
            }
            return BadRequest(validationRes);
        }

        [HttpDelete("[action]")]
       // [CustomAuthorizationFilter("DeleteAuthor")]
        public async Task<IActionResult> DeleteAuthor([FromQuery] Guid id)
        {
            bool isDelete = await _authorRepository.DeleteAsync(id);

            _memoryCache.Remove(id);
            _memoryCache.Remove(_Cache_Key);

            if (isDelete) return Ok("Author is deleted successfully");

            return BadRequest("Delete operation has been failed");
        }
    }
}
