using Application.DTOs.BookDTO;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        IEnumerable<BookGetDto> SearchBook(string text);
    }
}
