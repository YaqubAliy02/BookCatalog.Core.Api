using Domain.Enums;

namespace BookCatalog.Core.Web.Models.DTOs.Book
{
    public class GetAllBooksDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string BlobPath { get; set; }
        public string Thubnail { get; set; }
        public DateTime PublishedDate { get; set; }
        public ICollection<Author> Authors { get; set; }
        public BookCategories Categories { get; set; }
    }
}
