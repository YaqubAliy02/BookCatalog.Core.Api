using Domain.Enums;
using Microsoft.AspNetCore.Components.Forms;

namespace BookCatalog.Core.Web.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public IBrowserFile BlobPath { get; set; }
        public IBrowserFile Thubnail { get; set; }
        public DateOnly PublishedDate { get; set; }
        public ICollection<Author> Authors { get; set; }
        public BookCategories Categories { get; set; }
    }
}
