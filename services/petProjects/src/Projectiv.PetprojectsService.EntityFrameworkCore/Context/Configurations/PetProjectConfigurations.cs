using Microsoft.EntityFrameworkCore;
using Projectiv.PetprojectsService.Domain;
using Projectiv.PetprojectsService.Domain.Models;

namespace Projectiv.PetprojectsService.EntityFrameworkCore.Context.Configurations;

public static class PetProjectConfigurations
{
    public static void ConfigurePetProject(this ModelBuilder builder)
    {
        builder.Entity<PetProject>(x =>
        {
            x.ToTable(ProjectivPetProjectDbProperty.TablePrefix + "PetProject", ProjectivPetProjectDbProperty.DbSchema);
            x.HasKey(k => k.Id);
            x.Property(c => c.Name).IsRequired();
        });
    }
}