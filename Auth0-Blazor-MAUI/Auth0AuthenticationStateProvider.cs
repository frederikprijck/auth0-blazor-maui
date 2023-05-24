using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Auth0_Blazor_MAUI;

public class Auth0AuthenticationStateProvider : AuthenticationStateProvider
{
    private AuthenticationState currentUser = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    public Auth0AuthenticationStateProvider(Auth0Client client)
    {
        client.UserChanged += (newUser) =>
        {
            currentUser = new AuthenticationState(newUser);
            NotifyAuthenticationStateChanged(Task.FromResult(currentUser));
        };
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
        Task.FromResult(currentUser);
}

