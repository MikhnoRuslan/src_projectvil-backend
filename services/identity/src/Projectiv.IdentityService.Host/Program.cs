using System.Globalization;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Projectiv.IdentityService.ApplicationShared.Interfaces;
using Projectiv.IdentityService.DomainShared.Configuration;
using Projectiv.IdentityService.EntityFrameworkCore.Context;
using Projectvil.Shared.EntityFramework.Dependence;
using Projectvil.Shared.EntityFramework.Models;
using Projectvil.Shared.Infrastructures;
using Projectvil.Shared.Infrastructures.Configurations;
using Projectvil.Shared.Infrastructures.Constants;
using Projectvil.Shared.Infrastructures.Localization;

var builder = WebApplication.CreateBuilder(args);
const string routePrefix = "/api/identity-service";

var conf = builder.Configuration.Get<IdentityConfiguration>()!;
builder.Services.AddSingleton(conf);

builder.Services.AddDbContext<ProjectivIdentityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(conf.ConnectionStrings.UsersDatabase)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

SwaggerConfiguration.ConfigureWithAuth(
    context: builder,
    authority: conf.AuthServer.Authority,
    scopes: new Dictionary<string, string>()
    {
        { ProjectivConstants.Api.IdentityApi, ProjectivConstants.Microservice.IdentityService }
    },
    apiTitle: "IdentityService API",
    routePrefix: routePrefix);

builder.Services.AddDependencies();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddLocalization();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddProjectivAuthentication(conf.AuthServer.Authority, ProjectivConstants.Api.IdentityApi);
builder.Services.AddProjectivControllers(ProjectivConstants.Api.IdentityApi);
builder.Services.AddProjectivAuthorization(ProjectivConstants.Api.IdentityApi);

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

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(2);
});

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

builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

var app = builder.Build();

SeedDatabases(app);

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
        c.OAuth2RedirectUrl($"{conf.App.SelfUrl}/swagger/oauth2-redirect.html");
        c.OAuthClientId("swagger-client-id");
        c.OAuthClientSecret($"{conf.AuthServer.Secret}");
        c.OAuthAppName("IdentityService API");
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
app.UseErrorHandler();

app.UseTransaction<ProjectivIdentityDbContext>();

app.MapControllers();

app.Run();

static void SeedDatabases(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.CreateScope();
    var dbSeed = scope.ServiceProvider.GetService<IDataSeeder>();
    dbSeed?.SeedAsync().Wait();
}