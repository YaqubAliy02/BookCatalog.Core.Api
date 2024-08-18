using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repositories;
using Domain.Entities;

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
            _bookCatalogDbContext.Books.Attach(book);

            int result = await _bookCatalogDbContext.SaveChangesAsync();

            if(result > 0 ) return book;

            return null;
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

            if(book != null)
            {
                _bookCatalogDbContext.Books.Remove(book);

                int result = await _bookCatalogDbContext.SaveChangesAsync();

                if(result > 0) return true;
            }

            return false;
        }

        public async Task<IQueryable<Book>> GetAsync(Expression<Func<Book, bool>> expression)
        {
            return await Task.FromResult(_bookCatalogDbContext.Books.Where(expression));


        }

        public async Task<Book> GetByIdAsync(Guid id)
        {
            return await _bookCatalogDbContext.Books.FindAsync(id); 
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
