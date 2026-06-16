using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Abstractions;
using Microsoft.Extensions.Options;

namespace MobiGate.Api.Middleware;

public class ApiKeyAuthSchemeOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "ApiKey";
}

public class ApiKeyAuthHandler(IOptionsMonitor<ApiKeyAuthSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : AuthenticationHandler<ApiKeyAuthSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-Api-Key", out var apiKeyValues))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var apiKey = apiKeyValues.FirstOrDefault();
        var apiKeysSection = Context.RequestServices
            .GetRequiredService<IConfiguration>()
            .GetSection("ApiKeys");

        foreach (var child in apiKeysSection.GetChildren())
        {
            if (child.Value == apiKey)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, child.Key),
                    new Claim("auth_method", "api_key")
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
        }

        return Task.FromResult(AuthenticateResult.Fail("Invalid API key."));
    }
}
