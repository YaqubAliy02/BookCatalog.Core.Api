
using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repositories;
using Domain.Entities;

namespace Infrastracture.Services
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IBookCatalogDbContext _bookCatalogDbContext;

        public RoleRepository(IBookCatalogDbContext bookCatalogDbContext)
        {
            _bookCatalogDbContext = bookCatalogDbContext;
        }

        public async Task<Role> AddAsync(Role role)
        {
            _bookCatalogDbContext.Roles.Add(role);
            int result = await _bookCatalogDbContext.SaveChangesAsync();

            if(result > 0 ) return role;

            return null;
        }

        public async Task<IEnumerable<Role>> AddRangeAsync(IEnumerable<Role> roles)
        {
            _bookCatalogDbContext.Roles.AttachRange(roles);
            int result = await _bookCatalogDbContext.SaveChangesAsync();

            if(result > 0 ) return roles;

            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Role role = await _bookCatalogDbContext.Roles.FindAsync(id);
            if(role is not null)
            {
                _bookCatalogDbContext.Roles.Remove(role);
                int result = await _bookCatalogDbContext.SaveChangesAsync();

                if(result > 0) return true;
            }
            return false;
        }

        public Task<IQueryable<Role>> GetAsync(Expression<Func<Role, bool>> expression)
        {
            return Task.FromResult(_bookCatalogDbContext.Roles.Where(expression));
        }

        public async Task<Role> GetByIdAsync(Guid id)
        {
            return await _bookCatalogDbContext.Roles.FindAsync(id);
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            _bookCatalogDbContext.Roles.Update(role);
            int result = await _bookCatalogDbContext.SaveChangesAsync();
            if(result > 0) return role;

            return null;
        }
    }
}
