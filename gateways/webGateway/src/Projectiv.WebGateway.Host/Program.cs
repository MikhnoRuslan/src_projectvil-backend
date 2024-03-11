using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Rewrite;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Projectiv.WebGateway.ApplicationShared.Configuration;
using Projectvil.Shared.Infrastructures.Configurations;
using Projectvil.Shared.Infrastructures.Constants;

namespace Projectiv.WebGateway.Host;

public class Program
{
    public static void Main(string[] args)
    {
        var hostBuilder = WebApplication.CreateBuilder(args);
        var conf = hostBuilder.Configuration.Get<GatewayConfiguration>()!;

        hostBuilder.Host
            .ConfigureAppConfiguration((_, builder) =>
            {
                builder.AddJsonFile(
                    path: "ocelot.json",
                    optional: true,
                    reloadOnChange: true
                );
            })
            .ConfigureServices(s =>
            {
                s.AddOcelot();
            });

        hostBuilder.Services.AddSingleton(conf);
        hostBuilder.Services.AddEndpointsApiExplorer();
        
        SwaggerConfiguration.ConfigureWithAuth(
            context: hostBuilder,
            authority: conf.AuthServer.Authority,
            scopes: new Dictionary<string, string>()
            {
                { ProjectivConstants.Api.PetProjectApi, ProjectivConstants.Microservice.PetProjectService },
                { ProjectivConstants.Api.AuthApi, ProjectivConstants.Microservice.AuthService },
                { ProjectivConstants.Api.IdentityApi, ProjectivConstants.Microservice.IdentityService },
            },
            apiTitle: "GateWay API");
        
        hostBuilder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
        
        hostBuilder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(x =>
            {
                x.WithOrigins(conf.App.CorsOrigins
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .ToArray())
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        
        var app = hostBuilder.Build();

        app.UseStaticFiles();
        app.UseCors();
        
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var routes = hostBuilder.Configuration.GetSection("Routes").Get<List<OcelotConfiguration>>()!;
            var routedServices = routes
                .GroupBy(t => t.ServiceKey)
                .Select(r => r.First())
                .Distinct();

            foreach (var config in routedServices.OrderBy(q => q.ServiceKey))
            {
                var url = $"{config.DownstreamScheme}://{config.DownstreamHostAndPorts.FirstOrDefault()?.Host}:{config.DownstreamHostAndPorts.FirstOrDefault()?.Port}";
                if (!app.Environment.IsDevelopment())
                {
                    url = $"https://{config.DownstreamHostAndPorts.FirstOrDefault()?.Host}";
                }

                options.SwaggerEndpoint($"{url}/swagger/v1/swagger.json", $"{config.ServiceKey} API");
                options.OAuthClientId(conf.AuthServer.SwaggerClientId);
            }
        });
        
        app.UseRewriter(new RewriteOptions()
            // Regex for "", "/" and "" (whitespace)
            .AddRedirect("^(|\\|\\s+)$", "/swagger"));
        
        app.UseOcelot().Wait();
        app.Run();
    }
}