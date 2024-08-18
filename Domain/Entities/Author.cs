using Domain.Enums;

namespace Domain.Entities
{
    public class Author
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateOnly BirthDate { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
        public ICollection<Book> Books { get; set; }
    }
}
