using Microsoft.EntityFrameworkCore;
using Projectiv.IdentityService.Domain;
using Projectiv.IdentityService.Domain.Models.Auth;

namespace Projectiv.IdentityService.EntityFrameworkCore.Context.Configurations;

public static class AuthConfiguration
{
    public static void ConfigureIdentity(this ModelBuilder builder)
    {
        builder.Entity<Client>(x =>
        {
            x.ToTable(ProjectivIdentityDbProperty.TablePrefix + "Client", ProjectivIdentityDbProperty.DbSchema);
            x.HasKey(k => k.Id);
            x.HasIndex(c => c.ClientId);
            x.Property(c => c.ClientId).HasMaxLength(ProjectivIdentityDbProperty.ClientIdMaxLength);
            x.Property(c => c.ClientSecrets).HasMaxLength(ProjectivIdentityDbProperty.ClientSecretMaxLength);
        });

        builder.Entity<ApiResource>(x =>
        {
            x.ToTable(ProjectivIdentityDbProperty.TablePrefix + "ApiResource", ProjectivIdentityDbProperty.DbSchema);
            x.HasKey(k => k.Id);
            x.HasIndex(i => i.Name);
        });

        builder.Entity<IdentityResource>(x =>
        {
            x.ToTable(ProjectivIdentityDbProperty.TablePrefix + "IdentityResource", ProjectivIdentityDbProperty.DbSchema);
            x.HasKey(k => k.Id);
            x.Property(p => p.Name).HasMaxLength(ProjectivIdentityDbProperty.NameMaxLength);
            x.Property(p => p.DisplayName).HasMaxLength(ProjectivIdentityDbProperty.DisplayNameMaxLength);
        });
    }
}