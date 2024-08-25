using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstraction
{
    public interface IBookCatalogDbContext
    {
         DbSet<Book> Books { get; set; }
         DbSet<Author> Authors { get; set; }
         DbSet<User> Users { get; set; }
         DbSet<RefreshToken> RefreshTokens { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
