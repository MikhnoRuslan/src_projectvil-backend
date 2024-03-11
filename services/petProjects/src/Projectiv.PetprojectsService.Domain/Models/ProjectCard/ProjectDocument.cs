using System.ComponentModel.DataAnnotations;
using Projectvil.Shared.EntityFramework.Blob.Models;
using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.PetprojectsService.Domain.Models.ProjectCard;

public class ProjectDocument : AggregateModel<Guid>
{
    [Required]
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    
    [Required]
    public Guid DocumentId { get; set; }
    public Blob Document { get; set; }

    public ProjectDocument()
    {
        
    }

    public ProjectDocument(Guid id) : base(id)
    {
        
    }
}