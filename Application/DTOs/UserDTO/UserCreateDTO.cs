using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.UserDTO
{
    public class UserCreateDTO
    {
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }
        public Guid[] RolesId { get; set; }
    }
}
