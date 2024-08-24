using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class UserCredentials
    {
        [EmailAddress]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
