
using Domain.Enums;

namespace BookCatalog.Core.Web.Models
{
    public class CreateBookDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string BlobPath { get; set; }
        public string Thubnail { get; set; }
        public DateOnly PublishedDate { get; set; }
        public ICollection<Author> Authors { get; set; }
        public byte Categories { get; set; }
    }
}
