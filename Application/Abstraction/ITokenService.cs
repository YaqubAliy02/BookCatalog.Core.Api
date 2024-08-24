
using Domain.Entities;

namespace Application.Abstraction
{
    public interface ITokenService 
    {
        public string CreateToken(User user);
    }
}
