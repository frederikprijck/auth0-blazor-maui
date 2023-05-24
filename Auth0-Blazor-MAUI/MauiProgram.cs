using Microsoft.Extensions.Logging;
using Auth0_Blazor_MAUI.Data;
using IdentityModel.OidcClient;
using IdentityModel.Client;
using IdentityModel.OidcClient.Browser;
using Microsoft.AspNetCore.Components.Authorization;

namespace Auth0_Blazor_MAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<WeatherForecastService>();

        builder.Services.AddSingleton(new OidcClient(new()
        {
            Authority = "{AUTH0_DOMAIN}",
            ClientId = "{AUTH0_CLIENT_ID}",
            Scope = "openid profile",
            RedirectUri = "myapp://callback",
			Browser = new WebBrowserAuthenticator()
        }));

        builder.Services.AddAuthorizationCore();
        builder.Services.AddSingleton<Auth0Client>();
        builder.Services.AddScoped<AuthenticationStateProvider, Auth0AuthenticationStateProvider>();

        return builder.Build();
	}
}

