using System.Security.Claims;
using System.Text.Json.Serialization;
using Application.Abstraction;
using Application.Models;
using Application.UseCases.Authors.Command;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Application.UseCases.Accounts.Command
{
    public class RefreshTokenCommand : IRequest<ResponseCore<RefreshTokenCommandResult>>
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefereshToken { get; set; }
    }
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ResponseCore<RefreshTokenCommandResult>>
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public RefreshTokenCommandHandler(
            ITokenService tokenService, 
            IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<ResponseCore<RefreshTokenCommandResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var result = new ResponseCore<RefreshTokenCommandResult>();

            var principal = _tokenService.GetClaimsFromExpiredToken(request.AccessToken);
            string email = principal.FindFirstValue(ClaimTypes.Email);

            if (email is null)
            {
                result.ErrorMessage = new string[] { "Refresh token is not found" };
                result.StatusCode = 404;
                
                return result;
            }
            RefreshToken savedRefreshToken = _tokenService.Get(x =>
                x.Email == email && x.RefreshTokenValue == request.RefereshToken).AsNoTracking().FirstOrDefault();

            if (savedRefreshToken is null)
            {
                result.ErrorMessage = new string[] { "Refresh token or access token is invalid" };
                result.StatusCode = 420;

                return result;
            }
            if (savedRefreshToken.ExpiredDate < DateTime.UtcNow)
            {
                _tokenService.Delete(savedRefreshToken);
                result.ErrorMessage = new string[] { "Refresh token already expired please login again" };
                result.StatusCode = 405;

                return result;
            }
            Token newTokens = await _tokenService
                .CreateTokenFromRefresh(principal, savedRefreshToken);

            result.Result = _mapper.Map<RefreshTokenCommandResult>(newTokens);
            result.StatusCode = 200;
            return result;
        }
    }
    public class RefreshTokenCommandResult
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefereshToken { get; set; }
    }
}
