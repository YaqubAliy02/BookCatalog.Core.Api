using Domain.Enums;

namespace BookCatalog.Core.Web.Models.DTOs.Authors
{
    public class CreateAuthorDTO
    {
        public string FullName { get; set; }
        public string AuthorPhoto { get; set; }
        public DateTime BirthDate { get; set; }
        public byte Gender { get; set; }
        public string AboutAuthor { get; set; }
    }
}
