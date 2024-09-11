using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigationManager;

    public AuthorizationMessageHandler(ILocalStorageService localStorage, NavigationManager navigationManager)
    {
        _localStorage = localStorage;
        _navigationManager = navigationManager;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Retrieve the token from local storage
        var token = await _localStorage.GetItemAsync<string>("authToken");

        // Add token to request headers if it exists
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Proceed with the request
        return await base.SendAsync(request, cancellationToken);
    }
}
