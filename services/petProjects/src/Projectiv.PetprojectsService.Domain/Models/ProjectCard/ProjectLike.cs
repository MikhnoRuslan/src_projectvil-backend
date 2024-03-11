using System.ComponentModel.DataAnnotations;
using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.PetprojectsService.Domain.Models.ProjectCard;

public class ProjectLike : FullAggregateModel<Guid>
{
    [Required]
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    
    [Required]
    public Guid UserId { get; set; }

    public ProjectLike()
    {
        
    }

    public ProjectLike(Guid id) : base(id)
    {
        
    }
}