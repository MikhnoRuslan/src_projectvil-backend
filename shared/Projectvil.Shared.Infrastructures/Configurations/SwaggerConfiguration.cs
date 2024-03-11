using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Projectvil.Shared.Infrastructures.Configurations;

public static class SwaggerConfiguration
{
    public static void ConfigureWithAuth(
        WebApplicationBuilder context,
        string authority,
        Dictionary<string, string> scopes,
        string apiTitle,
        string apiVersion = "v1",
        string apiName = "v1",
        string routePrefix = ""
    )
    {
        context.Services.AddSwaggerWithOAuth(
            authority: authority,
            scopes: scopes,
            options =>
            {
                options.SwaggerDoc(apiName, new OpenApiInfo { Title = apiTitle, Version = apiVersion });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                options.DocumentFilter<SwaggerRoutePrefixFilter>(routePrefix);
            });
    }
}