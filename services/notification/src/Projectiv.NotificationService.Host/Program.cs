using System.Globalization;
using System.Net;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Projectiv.NotificationService.Domain.Handlers;
using Projectiv.NotificationService.DomainShared.Configuration.NotificationConfiguration;
using Projectiv.NotificationService.EntityFrameworkCore.Context;
using Projectvil.Shared.EntityFramework.Dependence;
using Projectvil.Shared.Infrastructures;
using Projectvil.Shared.Infrastructures.Configurations;
using Projectvil.Shared.Infrastructures.Constants;
using Projectvil.Shared.Infrastructures.Localization;
using Projectvil.Shared.RabbitMQ.Models.Identity;

var builder = WebApplication.CreateBuilder(args);

var conf = builder.Configuration.Get<NotificationConfiguration>()!;
builder.Services.AddSingleton(conf);

builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(conf.ConnectionStrings.Notification)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

SwaggerConfiguration.ConfigureWithAuth(
    context: builder,
    authority: conf.AuthServer.Authority,
    scopes: new Dictionary<string, string>()
    {
        { ProjectivConstants.Api.NotificationApi, ProjectivConstants.Microservice.NotificationService }
    },
    apiTitle: "NotificationService API");

builder.Services.AddDependencies();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();

builder.Services.AddProjectivAuthentication(conf.AuthServer.Authority, ProjectivConstants.Api.NotificationApi);
builder.Services.AddProjectivAuthorization(ProjectivConstants.Api.NotificationApi);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddConsumer<EmailConsumer>();
    
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

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.OAuth2RedirectUrl($"{conf.App.SelfUrl}/swagger/oauth2-redirect.html");
        c.OAuthClientId("swagger-client-id");
        c.OAuthClientSecret($"{conf.AuthServer.Secret}");
        c.OAuthAppName("NotificationService API");
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

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseErrorHandler();

app.UseTransaction<NotificationDbContext>();

app.MapControllers();
//app.MapGrpcService<NotificationGrpcService>();

app.Run();