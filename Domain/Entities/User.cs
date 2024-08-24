using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
