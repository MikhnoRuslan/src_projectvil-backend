using Microsoft.EntityFrameworkCore;
using Projectiv.PetprojectsService.Domain;
using Projectiv.PetprojectsService.Domain.Models.ProjectCard;

namespace Projectiv.PetprojectsService.EntityFrameworkCore.Context.Configurations;

public static class StatusConfiguration
{
    public static void ConfigureStatus(this ModelBuilder builder)
    {
        builder.Entity<Status>(x =>
        {
            x.ToTable(ProjectivPetProjectDbProperty.TablePrefix + "Statuses", ProjectivPetProjectDbProperty.DbSchema);
        });
    }
}