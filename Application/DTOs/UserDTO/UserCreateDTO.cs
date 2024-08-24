using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.UserDTO
{
    public class UserCreateDTO
    {
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
