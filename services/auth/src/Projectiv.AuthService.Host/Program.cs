using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Projectiv.AuthService.Application.Helpers;
using Projectiv.AuthService.DomainShared.Configurations;
using Projectiv.IdentityService.EntityFrameworkCore.Context;
using Projectvil.Shared.EntityFramework.Dependence;
using Projectvil.Shared.EntityFramework.Models;
using Projectvil.Shared.Infrastructures;
using Projectvil.Shared.Infrastructures.Configurations;
using Projectvil.Shared.Infrastructures.Constants;
using Projectvil.Shared.Infrastructures.Localization;

namespace Projectiv.AuthService.Host;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        const string routePrefix = "/api/auth-service";

        var conf = builder.Configuration.Get<ProjectivAuthConfiguration>()!;
        builder.Services.AddSingleton(conf);

        builder.Services.AddDbContext<ProjectivIdentityDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(conf.ConnectionStrings.UsersDatabase)));

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        SwaggerConfiguration.ConfigureWithAuth(
            context: builder,
            authority: conf.AuthServer.Authority,
            scopes: new Dictionary<string, string>()
            {
                { ProjectivConstants.Api.AuthApi, ProjectivConstants.Microservice.AuthService }
            },
            apiTitle: "AuthService API",
            routePrefix: routePrefix);

        builder.Services.AddDependencies();
        builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
        builder.Services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();
        
        builder.Services.AddLocalization();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddProjectivAuthentication(conf.AuthServer.Authority, ProjectivConstants.Api.AuthApi);

        builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(config =>
            {
                config.Password.RequireDigit = true;
                config.Password.RequireNonAlphanumeric = true;
                config.Password.RequireUppercase = true;
                config.User.AllowedUserNameCharacters = ProjectivConstants.AllowChars;
                config.Password.RequireLowercase = false;
                config.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ProjectivIdentityDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddIdentityServer(opt =>
            {
                opt.Cors.CorsPaths.Add(new PathString("/connect/token"));
            })
            .AddDeveloperSigningCredential()
            .AddClientStore<ClientStoreHelper>()
            .AddResourceStore<CustomResourceStoreHelper>()
            .AddAspNetIdentity<ApplicationUser>()
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidatorHelper>()
            .AddExtensionGrantValidator<GoogleGrantValidatorHelper>()
            .AddProfileService<ProjectivProfileHelper>()
            .AddJwtBearerClientAuthentication();

        builder.Services.AddCors(options =>
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

        builder.Services.AddRazorPages();

        builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

        var app = builder.Build();

        app.UsePathBase(new PathString(routePrefix));
        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseCors();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "swagger";
                c.OAuth2RedirectUrl($"http://{conf.AuthServer.Authority}/swagger/oauth2-redirect.html");
                c.OAuthClientId("swagger-client-id");
                c.OAuthAppName("AuthService API");
            });

            app.UseDeveloperExceptionPage();
        }
        
        var options = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US"))
        };

        app.UseRequestLocalization(options);
        app.UseStaticFiles();
        app.UseLocalization();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseIdentityServer();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
        });

        app.Run();
    }
}