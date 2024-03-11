using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Projectiv.IdentityService.EntityFrameworkCore.Context;
using Projectiv.PetprojectsService.EntityFrameworkCore.Context;

namespace Projectvil.DbMigrator;

public class ProjectivDbMigratorService
{
    public async Task MigrateAsync(IServiceScope scope, CancellationToken cancellationToken)
    {
        Console.WriteLine("Migrating Host side...");
        await MigrateAllDatabaseAsync(scope, cancellationToken);
    }

    private static async Task MigrateAllDatabaseAsync(IServiceScope scope, CancellationToken cancellationToken = default)
    {
        try
        {
            await MigrateDatabaseAsync<ProjectivIdentityDbContext>(scope, cancellationToken);
            await MigrateDatabaseAsync<PetProjectsDbContext>(scope, cancellationToken);
            
            Console.WriteLine("All databases have been successfully migrated");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static async Task MigrateDatabaseAsync<TDbContext>(IServiceScope scope, CancellationToken cancellationToken = default)
        where TDbContext: DbContext
    {
        Console.WriteLine($"Migrating {typeof(TDbContext).Name.Replace("DbContext", "")} database...");
        var dbContext = scope.ServiceProvider
            .GetRequiredService<TDbContext>();

        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}