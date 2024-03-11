using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projectiv.IdentityService.Domain.Models;
using Projectiv.IdentityService.Domain.Models.Auth;
using Projectiv.IdentityService.DomainShared.Configuration;
using Projectiv.IdentityService.EntityFrameworkCore.Context.Configurations;
using Projectvil.Shared.EntityFramework.Blob.Context;
using Projectvil.Shared.EntityFramework.Blob.Models;
using Projectvil.Shared.EntityFramework.Interfaces;
using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.IdentityService.EntityFrameworkCore.Context;

public class ProjectivIdentityDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IBlobContext
{
    private readonly IEntityTimestampUpdater _entityTimestampUpdater;

    public ProjectivIdentityDbContext(DbContextOptions<ProjectivIdentityDbContext> options, 
        IEntityTimestampUpdater entityTimestampUpdater)
        :base(options)
    {
        _entityTimestampUpdater = entityTimestampUpdater;
    }
    
    public DbSet<ExternalToken> ExternalTokens { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<IdentityResource> IdentityResources { get; set; }
    public DbSet<ApiResource> ApiResources { get; set; }
    public DbSet<Blob> Blobs { get; set; }
    public DbSet<BlobContainer> BlobContainers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var settings = IdentityConfiguration.BindSettings();
        optionsBuilder.UseNpgsql(settings.ConnectionStrings.UsersDatabase);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureIdentity();
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        _entityTimestampUpdater.UpdateTimestamps(ChangeTracker);
        return base.SaveChangesAsync(cancellationToken);
    }
}