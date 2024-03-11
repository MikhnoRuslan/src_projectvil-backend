using Microsoft.EntityFrameworkCore;
using Projectiv.PetprojectsService.Domain;
using Projectvil.Shared.EntityFramework.Translations;

namespace Projectiv.PetprojectsService.EntityFrameworkCore.Context.Configurations;

public static class TranslationConfiguration
{
    public static void ConfigureTranslation(this ModelBuilder builder)
    {
        builder.Entity<Translation>(x =>
        {
            x.ToTable(ProjectivPetProjectDbProperty.TablePrefix + "Translations", ProjectivPetProjectDbProperty.DbSchema);
            x.HasKey(k => new { k.Id, k.Language });
        });
    }
}