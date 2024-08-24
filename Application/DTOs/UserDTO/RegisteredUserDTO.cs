using Application.Models;
using Domain.Entities;

namespace Application.DTOs.UserDTO
{
    public class RegisteredUserDTO
    {
        public User User { get; set; }
        public Token UserTokens { get; set; }
    }
}
