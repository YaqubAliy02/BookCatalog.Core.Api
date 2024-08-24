using System.Linq;
using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repositories;
using Domain.Entities;

namespace Infrastracture.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IBookCatalogDbContext _bookCatalogDbContext;

        public UserRepository(IBookCatalogDbContext bookCatalogDbContext)
        {
            _bookCatalogDbContext = bookCatalogDbContext;
        }

        public async Task<User> AddAsync(User user)
        {
            await _bookCatalogDbContext.Users.AddAsync(user);
            int result = await _bookCatalogDbContext.SaveChangesAsync();

            if (result > 0) return user;

            return null;
        }

        public Task<IEnumerable<User>> AddRangeAsync(IEnumerable<User> entities)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            User user = await _bookCatalogDbContext.Users.FindAsync(id);

            if (user is not null)
            {
                _bookCatalogDbContext.Users.Remove(user);
            }

            int result = _bookCatalogDbContext.SaveChangesAsync().Result;

            if (result > 0) return true;
            return false;
        }

        public Task<IQueryable<User>> GetAsync(Expression<Func<User, bool>> expression)
        {
            return Task.FromResult(_bookCatalogDbContext.Users.Where(expression));
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _bookCatalogDbContext.Users.FindAsync(id);
        }

        public async Task<User> UpdateAsync(User user)
        {
            _bookCatalogDbContext.Users.Update(user);

            int result = await _bookCatalogDbContext.SaveChangesAsync();

            if (result > 0) return user;

            return null;
        }
    }
}
