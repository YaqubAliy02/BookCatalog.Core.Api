using Domain.Entities;

namespace Application.Repositories
{
    public interface IBookRepository : IRepository<Book> 
    {
        IEnumerable<Book> SearchBook(string text);
    }
}
