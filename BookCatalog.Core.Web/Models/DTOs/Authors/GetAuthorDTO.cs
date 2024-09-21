using Domain.Enums;
namespace BookCatalog.Core.Web.Models.DTOs.Authors
{
    public class GetAuthorDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string AboutAuthor { get; set; }
        public Gender Gender { get; set; }
        public string AuthorPhoto { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
