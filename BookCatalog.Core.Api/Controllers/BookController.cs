﻿using System.Text.Json;
using Application.DTOs.BookDTO;
using Application.Repositories;
using Application.UseCases.Books.Command;
using Application.UseCases.Books.Query;
using BookCatalog.Core.Api.Filters;
using Domain.Entities;
using FluentValidation;
using LazyCache;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace BookCatalog.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class BookController : ApiControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IValidator<Book> _validator;
        private readonly IAppCache _lazyCache;
        private readonly IDistributedCache _distributedCache;
        private readonly IMediator _mediator;

        private readonly string _Key = "MyLazyCache";
        public BookController(
            IBookRepository bookRepository,
            IValidator<Book> validator,
            IAuthorRepository authorRepository,
            IAppCache lazyCache,
            IDistributedCache distributedCache,
            IMediator mediator)
        {
            _bookRepository = bookRepository;
            _validator = validator;
            _authorRepository = authorRepository;
            _lazyCache = lazyCache;
            _distributedCache = distributedCache;
            _mediator = mediator;
        }

        [HttpGet("[action]")]
/*        [CustomAuthorizationFilter("GetAllBooks")]*/
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

        [HttpGet("[action]f/{id}")]
  /*      [CustomAuthorizationFilter("GetBookById")]*/
        public async Task<IActionResult> GetBookByIdAsync(GetBookByIdQuery getBookByIdQuery)
        {
            return await _mediator.Send(getBookByIdQuery);
        }

        [HttpPost("[action]")]
   /*     [CustomAuthorizationFilter("CreateBook")]*/
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
        //[CustomAuthorizationFilter("UpdateBook")]
        public async Task<IActionResult> UpdateBookAsync([FromBody] UpdateBookCommand updateBookCommand)
        {
            return await _mediator.Send(updateBookCommand); 
        }

        [HttpDelete("[action]")]
        //[CustomAuthorizationFilter("DeleteBook")]
        public async Task<IActionResult> DeleteBook([FromQuery] DeleteBookCommand deleteBookCommand)
        {
            return await _mediator.Send(deleteBookCommand);
        }

        [HttpGet("[action]")]
        public IActionResult SearchBook(string searchText)
        {
            return Ok(_bookRepository.SearchBook(searchText));
        }
    }
}
