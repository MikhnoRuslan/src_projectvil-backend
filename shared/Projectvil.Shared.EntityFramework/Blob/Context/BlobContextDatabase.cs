using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Projectvil.Shared.EntityFramework.Blob.Models;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectvil.Shared.EntityFramework.Blob.Context;

public class BlobContextDatabase : DbContext, IBlobContextDatabase, ITransientDependence
{
    public DbSet<Models.Blob> Blobs { get; set; }
    public DbSet<BlobContainer> BlobContainers { get; set; }
    
    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    public ValueTask<EntityEntry<TEntity>> AddEntityAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        return base.AddAsync(entity, cancellationToken);
    }

    public EntityEntry<TEntity> RemoveEntity<TEntity>(TEntity entity) where TEntity : class
    {
        return base.Remove(entity);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var currentDir = Directory.GetCurrentDirectory();
        var baseDirectory = Directory.GetParent(currentDir)!.ToString();
        var appSettingsPath = Directory.GetFiles(baseDirectory, "appsettings.json", SearchOption.AllDirectories).FirstOrDefault();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(appSettingsPath)!)
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        
        var connectionString = configuration.GetConnectionString("BlobStorage");
        
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}