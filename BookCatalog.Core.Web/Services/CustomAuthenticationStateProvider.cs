using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookCatalog.Core.Web.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationState _anonymous;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public async Task LogoutAsync()
        {
            // Clear the token from local storage
            await _localStorage.RemoveItemAsync("authToken");

            // Notify the authentication state has changed
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Get the token from local storage
            var authToken = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrEmpty(authToken))
            {
                return _anonymous;
            }

            // Create a ClaimsPrincipal based on the token
            var claims = new[] { new Claim(ClaimTypes.Name, "User") }; // Simplified, you should parse the token here
            var identity = new ClaimsIdentity(claims, "jwtAuthType");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
    }

}
