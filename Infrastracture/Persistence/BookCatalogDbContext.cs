using Application.Abstraction;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastracture.Persistence
{
    public class BookCatalogDbContext : DbContext, IBookCatalogDbContext
    {
        private readonly IConfiguration _configuration;
        public BookCatalogDbContext(DbContextOptions<BookCatalogDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(option => option.Email).IsUnique();
        }
    }
}
