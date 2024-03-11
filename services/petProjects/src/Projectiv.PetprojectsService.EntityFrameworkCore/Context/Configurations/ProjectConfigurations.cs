using Microsoft.EntityFrameworkCore;
using Projectiv.PetprojectsService.Domain;
using Projectiv.PetprojectsService.Domain.Models.ProjectCard;

namespace Projectiv.PetprojectsService.EntityFrameworkCore.Context.Configurations;

public static class ProjectConfigurations
{
    public static void ConfigureProject(this ModelBuilder builder)
    {
        builder.Entity<Project>(x =>
        {
            x.ToTable(ProjectivPetProjectDbProperty.TablePrefix + "Projects", ProjectivPetProjectDbProperty.DbSchema);
        });
        
        builder.Entity<ProjectDocument>(x =>
        {
            x.ToTable(ProjectivPetProjectDbProperty.TablePrefix + "ProjectDocuments", ProjectivPetProjectDbProperty.DbSchema);
        });
        
        builder.Entity<ProjectLike>(x =>
        {
            x.ToTable(ProjectivPetProjectDbProperty.TablePrefix + "ProjectLikes", ProjectivPetProjectDbProperty.DbSchema);
        });
    }
}