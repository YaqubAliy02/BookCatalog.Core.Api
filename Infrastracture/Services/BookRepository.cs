using System.Linq.Expressions;
using Application.Abstraction;
using Application.DTOs.BookDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace Infrastracture.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly IBookCatalogDbContext _bookCatalogDbContext;
        private readonly IMapper _mapper;

        public BookRepository(IBookCatalogDbContext bookCatalogDbContext,
            IMapper mapper)
        {
            _bookCatalogDbContext = bookCatalogDbContext;
            _mapper = mapper;
        }

        public async Task<Book> AddAsync(Book book)
        {
            var authorIds = book.Authors.Select(a => a.Id).ToList();
            var authors = await _bookCatalogDbContext.Authors
                .Where(a => authorIds.Contains(a.Id))
                .ToListAsync();

            book.Authors.Clear();
            foreach (var author in authors)
            {
                book.Authors.Add(author);
            }

            await _bookCatalogDbContext.Books.AddAsync(book);

            int result = await _bookCatalogDbContext.SaveChangesAsync();

            return result > 0 ? book : null;
        }


        public async Task<IEnumerable<Book>> AddRangeAsync(IEnumerable<Book> entities)
        {
            _bookCatalogDbContext.Books.AttachRange(entities);

            int result = await _bookCatalogDbContext.SaveChangesAsync();

            if (result > 0) return entities;

            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Book book = await _bookCatalogDbContext.Books.FindAsync(id);

            if (book is not null)
            {
                _bookCatalogDbContext.Books.Remove(book);
            }
            int result = _bookCatalogDbContext.SaveChangesAsync().Result;

            if (result > 0) return true;

            return false;
        }

        public async Task<IQueryable<Book>> GetAsync(Expression<Func<Book, bool>> expression)
        {
            return _bookCatalogDbContext.Books.Where(expression).Include(x => x.Authors);
        }

        public async Task<Book> GetByIdAsync(Guid id)
        {
            return await _bookCatalogDbContext.Books
                .Include(x => x.Authors)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public IEnumerable<BookGetDto> SearchBook(string text)
        {
            var books = _bookCatalogDbContext.Books.ToList();

            if (string.IsNullOrWhiteSpace(text))
            {
                return _mapper.Map<IEnumerable<BookGetDto>>(books);
            }
            else
            {
                var result = books.Where(x => x.Name.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                                         x.ISBN.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                                         x.Description.Contains(text, StringComparison.OrdinalIgnoreCase)).ToList();

                return _mapper.Map<IEnumerable<BookGetDto>>(result);
            }
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            _bookCatalogDbContext.Books.Update(book);
            int result = await _bookCatalogDbContext.SaveChangesAsync();

            if (result > 0) return book;

            return null;
        }
    }
}
