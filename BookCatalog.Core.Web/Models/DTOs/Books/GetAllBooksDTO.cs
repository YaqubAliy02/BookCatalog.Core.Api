using BookCatalog.Core.Web.Models.DTOs.Authors;
using Domain.Enums;

namespace BookCatalog.Core.Web.Models.DTOs.Books
{
    public class GetAllBooksDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string Thubnail { get; set; }
        public string BlobPath { get; set; }
        public DateTime PublishedDate { get; set; }
        public ICollection<GetAuthorDTO> Authors { get; set; }
        public BookCategories Categories { get; set; }
    }
}