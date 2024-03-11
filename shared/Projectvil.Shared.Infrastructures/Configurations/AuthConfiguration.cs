using IdentityServer4;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Projectvil.Shared.Infrastructures.Configurations;

public static class AuthConfiguration
{
    public static void AddProjectivAuthorization(this IServiceCollection service, string apiName)
    {
        service.AddAuthorization(options =>
        {
            options.AddPolicy("DefaultPolicy", policy =>
                policy.RequireClaim("scope", apiName));
        });
    }

    public static void AddProjectivAuthentication(this IServiceCollection service, string authUrl, string apiName = null)
    {
        service.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = authUrl;
                options.RequireHttpsMetadata = true;
                options.Audience = apiName;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });
    }

    public static void AddProjectivControllers(this IServiceCollection service, string apiName)
    {
        service.AddControllers(options =>
        {
            var defaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("scope", apiName)
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .Build();

            options.Filters.Add(new AuthorizeFilter(defaultPolicy));
        });
    }

    public static void AddOpenIdConfig(this IServiceCollection service, string authUrl)
    {
        service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies", options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(365);
            })
            .AddOpenIdConnect(IdentityServerConstants.DefaultCookieAuthenticationScheme, options =>
            {
                options.Authority = authUrl;
                options.RequireHttpsMetadata = true;

                options.ClientId = "swagger-client-id";
                options.ClientSecret = "ProjectivSecret";
                options.ResponseType = "code";

                options.SaveTokens = true;
            });
    }
}