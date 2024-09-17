using Domain.Entities;

namespace Application.Repositories
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<List<Author>> GetAllByIdsAsync(IEnumerable<Guid> authorsId);
    }
}
