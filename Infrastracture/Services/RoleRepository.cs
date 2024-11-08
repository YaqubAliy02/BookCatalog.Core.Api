
using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

            if (result > 0) return role;

            return null;
        }

        public async Task<IEnumerable<Role>> AddRangeAsync(IEnumerable<Role> roles)
        {
            _bookCatalogDbContext.Roles.AttachRange(roles);
            int result = await _bookCatalogDbContext.SaveChangesAsync();

            if (result > 0) return roles;

            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Role role = await _bookCatalogDbContext.Roles.FindAsync(id);
            if (role is not null)
            {
                _bookCatalogDbContext.Roles.Remove(role);
                int result = await _bookCatalogDbContext.SaveChangesAsync();

                if (result > 0) return true;
            }
            return false;
        }

        public async Task<IQueryable<Role>> GetAsync(Expression<Func<Role, bool>> expression)
        {
            return _bookCatalogDbContext.Roles.Where(expression).Include(x => x.Permissions);
        }

        public async Task<Role> GetByIdAsync(Guid id)
        {
            return _bookCatalogDbContext.Roles.Where(x => x.RoleId == id).Include(x => x.Permissions).SingleOrDefault();
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _bookCatalogDbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task<Role> UpdateAsync(Role updatedRole)
        {
            var existingRole = await GetByIdAsync(updatedRole.RoleId);

            if (existingRole is not null)
            {
                existingRole.RoleName = updatedRole.RoleName;

                existingRole.Permissions.Clear();

                foreach (var permission in updatedRole.Permissions)
                {
                    var existingPermission = _bookCatalogDbContext.Permissions.Find(permission.PermissionId);

                    if (existingPermission is not null)
                    {
                        existingRole.Permissions.Add(existingPermission);
                    }
                }

                int result = await _bookCatalogDbContext.SaveChangesAsync();
                if (result > 0) return updatedRole;
            }
            return null;
        }

    }
}
