using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Projectiv.PetprojectsService.DomainShared.Configuration.ModelConfigurations.ProjectCard;
using Projectvil.Shared.EntityFramework.Models;
using Projectvil.Shared.EntityFramework.Translations;

namespace Projectiv.PetprojectsService.Domain.Models.ProjectCard;

public class Domain : Entity<Guid>
{
    [Required]
    [MaxLength(ProjectCardConfiguration.MaxDomainNameLength)]
    public Guid NameTranslationId { get; set; }

    [NotMapped]
    public string NameTranslation { get; set; }

    public Domain()
    {
        
    }

    public Domain(Guid id) : base(id)
    {
        
    }
}