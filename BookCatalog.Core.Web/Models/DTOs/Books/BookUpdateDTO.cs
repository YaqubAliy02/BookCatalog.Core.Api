using Domain.Enums;

namespace BookCatalog.Core.Web.Models.DTOs.Books
{
    public class BookUpdateDTO
    {
        public string Name { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string BlobPath { get; set; }
        public string Thubnail { get; set; }
        public DateTime PublishedDate { get; set; }
        public Guid[] AuthorsId { get; set; }
        public byte Categories { get; set; }
    }
}
