using Microsoft.EntityFrameworkCore;
using Projectiv.PetprojectsService.Domain;

namespace Projectiv.PetprojectsService.EntityFrameworkCore.Context.Configurations;

public static class DomainConfiguration
{
    public static void ConfigureDomain(this ModelBuilder builder)
    {
        builder.Entity<Domain.Models.ProjectCard.Domain>(x =>
        {
            x.ToTable(ProjectivPetProjectDbProperty.TablePrefix + "Domains", ProjectivPetProjectDbProperty.DbSchema);
        });
    }
}