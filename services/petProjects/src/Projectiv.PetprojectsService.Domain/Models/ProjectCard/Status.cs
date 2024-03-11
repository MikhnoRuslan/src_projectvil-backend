using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.PetprojectsService.Domain.Models.ProjectCard;

public class Status : Entity<Guid>
{
    [Required]
    public Guid NameTranslationId { get; set; }

    [NotMapped]
    public string NameTranslation { get; set; }

    public Status()
    {
        
    }

    public Status(Guid id) : base(id)
    {
        
    }
}