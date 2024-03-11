using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Projectiv.PetprojectsService.DomainShared.Configuration.PetProjectConfiguration;
using Projectiv.PetprojectsService.EntityFrameworkCore.Context;
using Projectvil.Shared.EntityFramework.Dependence;
using Projectvil.Shared.Infrastructures;
using Projectvil.Shared.Infrastructures.Configurations;
using Projectvil.Shared.Infrastructures.Constants;
using Projectvil.Shared.Infrastructures.Localization;

var builder = WebApplication.CreateBuilder(args);
const string routePrefix = "/api/project-service";

var conf = builder.Configuration.Get<PetProjectConfiguration>()!;
builder.Services.AddSingleton(conf);

builder.Services.AddDbContext<PetProjectsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(conf.ConnectionStrings.PetProjects)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

SwaggerConfiguration.ConfigureWithAuth(
    context: builder,
    authority: conf.AuthServer.Authority,
    scopes: new Dictionary<string, string>()
    {
        { ProjectivConstants.Api.PetProjectApi, ProjectivConstants.Microservice.PetProjectService }
    },
    apiTitle: "PetProjectService API",
    routePrefix: routePrefix);

builder.Services.AddDependencies();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();

builder.Services.AddLocalization();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddProjectivAuthentication(conf.AuthServer.Authority, ProjectivConstants.Api.PetProjectApi);
builder.Services.AddProjectivAuthorization(ProjectivConstants.Api.PetProjectApi);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(z =>
    {
        z.WithOrigins(
                conf.App.CorsOrigins
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .ToArray()
            )
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

ApplyMigrations(app);

app.UsePathBase(new PathString(routePrefix));

// Configure the HTTP request pipeline.

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
        c.OAuthAppName("PetProjectService API");
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

app.UseTransaction<PetProjectsDbContext>();

app.MapControllers();
/*app.MapGrpcService<StatusGrpcService>();
app.MapGrpcService<DomainGrpcService>();
app.MapGrpcService<ProjectGrpcService>();
app.MapGrpcService<ProjectBlobGrpcService>();*/

app.Run();

static void ApplyMigrations(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.CreateScope();
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<PetProjectsDbContext>();
    context.Database.Migrate();
}