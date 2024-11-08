using Domain.Entities;

namespace Application.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task UpdatePasswordAsync(User user);
    }
}
