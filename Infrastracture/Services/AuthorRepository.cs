using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repositories;
using Domain.Entities;

namespace Infrastracture.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly IBookCatalogDbContext _bookCatalogDbContext;

        public AuthorRepository(IBookCatalogDbContext bookCatalogDbContext)
        {
            _bookCatalogDbContext = bookCatalogDbContext;
        }

        public async Task<Author> AddAsync(Author author)
        {
            _bookCatalogDbContext.Authors.Attach(author);
            int result = await _bookCatalogDbContext.SaveChangesAsync();

            if (result > 0) return author;

            return null;
        }

        public async Task<IEnumerable<Author>> AddRangeAsync(IEnumerable<Author> entities)
        {
            _bookCatalogDbContext.Authors.AttachRange(entities);
            int result = await _bookCatalogDbContext.SaveChangesAsync();

            if(result > 0) return entities;

            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Author author = await _bookCatalogDbContext.Authors.FindAsync(id);

            if (author is not null)
            {
                _bookCatalogDbContext.Authors.Remove(author);
            }

            int result = _bookCatalogDbContext.SaveChangesAsync().Result;

            if (result > 0) return true;
            return false;
        }

        public IQueryable<Author> GetAsync(Expression<Func<Author, bool>> expression)
        {
            return _bookCatalogDbContext.Authors.Where(expression);
        }

        public async Task<Author> GetByIdAsync(Guid id)
        {
            return await _bookCatalogDbContext.Authors.FindAsync(id);
        }

        public async Task<Author> UpdateAsync(Author author)
        {
            _bookCatalogDbContext.Authors.Update(author);

            int result = await _bookCatalogDbContext.SaveChangesAsync();

            if (result > 0) return author;

            return null;
        }
    }
}
