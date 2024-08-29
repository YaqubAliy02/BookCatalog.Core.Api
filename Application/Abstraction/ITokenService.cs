
using System.Linq.Expressions;
using System.Security.Claims;
using Application.Models;
using Domain.Entities;

namespace Application.Abstraction
{
    public interface ITokenService
    {
        public Task<Token> CreateTokensAsync(User user);
        public Task<Token> CreateTokenFromRefresh(ClaimsPrincipal principal, RefreshToken savedRefreshToken);
        public ClaimsPrincipal GetClaimsFromExpiredToken(string token);

        public Task<bool> AddRefreshToken(RefreshToken refreshToken);
        public bool Update(RefreshToken refreshToken);
        public IQueryable<RefreshToken> Get(Expression<Func<RefreshToken, bool>> expression);
        public bool Delete(RefreshToken refreshToken);



    }
}
