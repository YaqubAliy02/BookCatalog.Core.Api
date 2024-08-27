using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using Application.Abstraction;
using Application.Extensions;
using Application.Models;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IBookCatalogDbContext _bookCatalogDbContext;
        private readonly int _refreshTokenLifetime;
        private readonly int _accessTokenLifetime;

        public TokenService(IConfiguration configuration,
            IBookCatalogDbContext bookCatalogDbContext)
        {
            _configuration = configuration;
            _bookCatalogDbContext = bookCatalogDbContext;
            _refreshTokenLifetime = int.Parse(configuration["JWT:RefreshTokenLifetime"]);
            _accessTokenLifetime = int.Parse(configuration["JWT:AccessTokenLifeTime"]);
        }

        public async Task<Token> CreateTokensAsync(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            Role eachRole;
            List<string> permissions = new();
            foreach (Role role in user.Roles)
            {
                eachRole = _bookCatalogDbContext.Roles
                    .Where(x => x.RoleId.Equals(role.RoleId))
                    .Include(x => x.Permissions).SingleOrDefault();

                foreach (Permission permission in eachRole.Permissions)
                {
                    if (!permissions.Contains(permission.PermissionName))
                    {
                        permissions.Add(permission.PermissionName);
                        claims.Add(new Claim(ClaimTypes.Role, permission.PermissionName));
                    }
                }
            }


            claims = claims.Distinct().ToList();
            Token tokens = CreateToken(claims);

            var savedRefreshToken = Get(x => x.Email == user.Email).FirstOrDefault();
            if (savedRefreshToken is null)
            {
                var refreshToken = new RefreshToken()
                {
                    ExpiredDate = DateTime.UtcNow.AddMinutes(_refreshTokenLifetime),
                    RefreshTokenValue = tokens.RefereshToken,
                    Email = user.Email
                };
                await AddRefreshToken(refreshToken);
            }
            else
            {
                savedRefreshToken.RefreshTokenValue = tokens.RefereshToken;
                savedRefreshToken.ExpiredDate = DateTime.UtcNow.AddMinutes(_refreshTokenLifetime);
                Update(savedRefreshToken);
            }

            return tokens;
        }
        public Token CreateToken(IEnumerable<Claim> claims)
        {

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:IssuerKey"],
                audience: _configuration["JWT:AudienceKey"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JWT:AccessTokenLifeTime"])),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
                        SecurityAlgorithms.HmacSha256Signature));

            Token tokens = new()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefereshToken = GenerateRefreshToken()
            };

            return tokens;
        }

        private string GenerateRefreshToken()
        {
            return (DateTime.UtcNow.ToString() + _configuration["JWT:Key"]).GetHash();
        }

        public Task<Token> CreateTokenFromRefresh(ClaimsPrincipal principal, RefreshToken savedRefreshToken)
        {
            Token tokens = CreateToken(principal.Claims);
            savedRefreshToken.RefreshTokenValue = tokens.RefereshToken;
            savedRefreshToken.ExpiredDate = DateTime.UtcNow.AddMinutes(_refreshTokenLifetime);

            Update(savedRefreshToken);
            return Task.FromResult(tokens);
        }

        public ClaimsPrincipal GetClaimsFromExpiredToken(string token)
        {
            byte[] key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            var tokenParams = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:AudienceKey"],
                ValidateIssuer = true,
                ValidateLifetime = false,
                ValidIssuer = _configuration["JWT:IssuerKey"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler tokenHandler = new();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenParams, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken is null)
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }

        public async Task<bool> AddRefreshToken(RefreshToken refreshToken)
        {
            await _bookCatalogDbContext.RefreshTokens.AddAsync(refreshToken);
            await _bookCatalogDbContext.SaveChangesAsync();

            return true;
        }

        public bool Update(RefreshToken refreshToken)
        {
            _bookCatalogDbContext.RefreshTokens.Update(refreshToken);
            _bookCatalogDbContext.SaveChangesAsync();

            return true;
        }

        public IQueryable<RefreshToken> Get(Expression<Func<RefreshToken, bool>> expression)
        {
            return _bookCatalogDbContext.RefreshTokens.Where(expression);
        }

        public bool Delete(RefreshToken refreshToken)
        {
            _bookCatalogDbContext.RefreshTokens.Remove(refreshToken);
            _bookCatalogDbContext.SaveChangesAsync();

            return true;
        }
    }
}
