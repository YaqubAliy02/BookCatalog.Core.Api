using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly IBookCatalogDbContext _bookCatalogDbContext;

        public BookRepository(IBookCatalogDbContext bookCatalogDbContext)
        {
            _bookCatalogDbContext = bookCatalogDbContext;
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

            if(result > 0 ) return entities;

            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
           Book book = await _bookCatalogDbContext.Books.FindAsync(id);

            if(book is not null)
            {
                _bookCatalogDbContext.Books.Remove(book);
            }
            int result = _bookCatalogDbContext.SaveChangesAsync().Result;

            if (result > 0) return true;

            return false;
        }

        public async Task<IQueryable<Book>> GetAsync(Expression<Func<Book, bool>> expression)
        {
            return await Task.FromResult(_bookCatalogDbContext.Books.Where(expression));
        }

        public async Task<Book> GetByIdAsync(Guid id)
        {
            return await _bookCatalogDbContext.Books
                .Include(x => x.Authors)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            _bookCatalogDbContext.Books.Update(book);
            int result = await _bookCatalogDbContext.SaveChangesAsync();

            if(result > 0) return book;

            return null;
        }
    }
}
