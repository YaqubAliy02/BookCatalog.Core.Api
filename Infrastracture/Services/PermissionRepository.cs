
using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repositories;
using Domain.Entities;

namespace Infrastracture.Services
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IBookCatalogDbContext _bookCatalogDbContext;

        public PermissionRepository(IBookCatalogDbContext bookCatalogDbContext)
        {
            _bookCatalogDbContext = bookCatalogDbContext;
        }

        public async Task<Permission> AddAsync(Permission permission)
        {
            _bookCatalogDbContext.Permissions.Add(permission);
            int result = await _bookCatalogDbContext.SaveChangesAsync();

            if (result > 0) return permission;

            return null;
        }

        public async Task<IEnumerable<Permission>> AddRangeAsync(IEnumerable<Permission> permissions)
        {
            _bookCatalogDbContext.Permissions.AttachRange(permissions);
            int result = await _bookCatalogDbContext.SaveChangesAsync();

            if (result > 0) return permissions;

            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Permission permission = await _bookCatalogDbContext.Permissions.FindAsync(id);
            if (permission is not null)
            {
                _bookCatalogDbContext.Permissions.Remove(permission);
                int result = await _bookCatalogDbContext.SaveChangesAsync();

                if (result > 0) return true;
            }
            return false;
        }

        public Task<IQueryable<Permission>> GetAsync(Expression<Func<Permission, bool>> expression)
        {
            return Task.FromResult(_bookCatalogDbContext.Permissions.Where(expression));
        }

        public async Task<Permission> GetByIdAsync(Guid id)
        {
            return await _bookCatalogDbContext.Permissions.FindAsync(id);
        }

        public async Task<Permission> UpdateAsync(Permission permission)
        {
            _bookCatalogDbContext.Permissions.Update(permission);
            int result = await _bookCatalogDbContext.SaveChangesAsync();
            if (result > 0) return permission;

            return null;
        }
    }
}
