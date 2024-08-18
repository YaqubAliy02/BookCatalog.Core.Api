using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs.BookDTO
{
    public class BookUpdateDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public DateOnly PublishedDate { get; set; }
        public Guid[] Authors { get; set; }
        public BookCategories Categories { get; set; }
    }
}
