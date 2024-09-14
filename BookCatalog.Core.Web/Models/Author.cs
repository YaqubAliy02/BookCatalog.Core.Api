using Domain.Enums;

namespace BookCatalog.Core.Web.Models
{
    public class Author
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string AboutAuthor { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
        public string AuthorPhoto { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
