using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Projectvil.Shared.Infrastructures.Configurations;

public static class SwaggerGenServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerWithOAuth(
        this IServiceCollection services,
        [NotNull] string authority,
        [NotNull] Dictionary<string, string> scopes,
        Action<SwaggerGenOptions> setupAction = null,
        string authorizationEndpoint = "/connect/authorize",
        string tokenEndpoint = "/connect/token")
    {
        var authorizationUrl = new Uri($"{authority.TrimEnd('/')}{authorizationEndpoint}");
        var tokenUrl = new Uri($"{authority.TrimEnd('/')}{tokenEndpoint}");
        
        return services
            .AddSwaggerGen(
                options =>
                {
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = authorizationUrl,
                                Scopes = scopes,
                                TokenUrl = tokenUrl
                            }
                        }
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "oauth2"
                                    }
                                },
                                Array.Empty<string>()
                            }
                    });

                    setupAction?.Invoke(options);
                });
    }
}