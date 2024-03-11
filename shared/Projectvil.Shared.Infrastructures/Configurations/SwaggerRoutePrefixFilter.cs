using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Projectvil.Shared.Infrastructures.Configurations;

public class SwaggerRoutePrefixFilter : IDocumentFilter
{
    private readonly string _prefix;

    public SwaggerRoutePrefixFilter(string prefix)
    {
        _prefix = prefix;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var newPaths = new OpenApiPaths();
        
        foreach (var path in swaggerDoc.Paths)
        {
            var newPathKey = $"{_prefix}{path.Key}";
            newPaths.Add(newPathKey, path.Value);
        }

        swaggerDoc.Paths = newPaths;
    }
}