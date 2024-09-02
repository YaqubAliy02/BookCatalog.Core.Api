using System.Linq.Expressions;
using Application.Abstraction;
using Application.Extensions;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
            user.Password = user.Password.GetHash();

            _bookCatalogDbContext.Users.Add(user);

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

        public async Task<IQueryable<User>> GetAsync(Expression<Func<User, bool>> expression)
        {
            return _bookCatalogDbContext.Users.Where(expression).Include(x => x.Roles);
        }

        public Task<User> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_bookCatalogDbContext.Users
                .Where(x => x.Id.Equals(id))
                .Include(x => x.Roles)
                .SingleOrDefault());
        }

        public async Task<User> UpdateAsync(User user)
        {
            var exisingUser = await GetByIdAsync(user.Id);

            if (exisingUser is not null)
            {
                exisingUser.FullName = user.FullName;
                exisingUser.Email = user.Email;

                exisingUser.Roles.Clear();

                foreach (var permission in user.Roles)
                {
                    var existingPermission = _bookCatalogDbContext.Roles.Find(permission.RoleId);

                    if (existingPermission is not null)
                    {
                        exisingUser.Roles.Add(existingPermission);
                    }
                }
                int result = await _bookCatalogDbContext.SaveChangesAsync();

                if (result > 0) return user;
            }

            return null;
        }
    }
}
