using System.Security.Claims;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;

namespace Auth0_Blazor_MAUI;

public class Auth0Client
{
    public event Action<ClaimsPrincipal>? UserChanged;

    private readonly OidcClient oidcClient;

    public Auth0Client(OidcClient client)
    {
        oidcClient = client;
    }

    public async Task<LoginResult> LogInAsync()
    {
        var result = await oidcClient.LoginAsync();

        UserChanged(result.User);

        return result;
    }

    public async void LogOut()
    {
        var logoutParameters = new Dictionary<string, string>
        {
          {"client_id", oidcClient.Options.ClientId },
          {"returnTo", oidcClient.Options.RedirectUri }
        };

        var logoutRequest = new LogoutRequest();
        var endSessionUrl = new RequestUrl($"{oidcClient.Options.Authority}/v2/logout")
          .Create(new Parameters(logoutParameters));
        var browserOptions = new BrowserOptions(endSessionUrl, oidcClient.Options.RedirectUri)
        {
            Timeout = TimeSpan.FromSeconds(logoutRequest.BrowserTimeout),
            DisplayMode = logoutRequest.BrowserDisplayMode
        };

        await oidcClient.Options.Browser.InvokeAsync(browserOptions);

        UserChanged(new ClaimsPrincipal(new ClaimsIdentity()));
    }
}

