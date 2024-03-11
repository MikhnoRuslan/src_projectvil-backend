using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Projectvil.DbMigrator;

public class DbMigratorHostedService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public DbMigratorHostedService(IServiceScopeFactory scopeFactory,
        IHostApplicationLifetime hostApplicationLifetime)
    {
        _scopeFactory = scopeFactory;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ProjectivDbMigratorService>();
        await service.MigrateAsync(scope, cancellationToken);
        
        _hostApplicationLifetime.StopApplication();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}