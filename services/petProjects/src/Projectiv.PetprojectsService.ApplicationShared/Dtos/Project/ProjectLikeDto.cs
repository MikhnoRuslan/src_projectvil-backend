using System.ComponentModel.DataAnnotations;

namespace Projectiv.PetprojectsService.ApplicationShared.Dtos.Project;

public class ProjectLikeDto
{
    [Required]
    public Guid ProjectId { get; set; }

    [Required]
    public int Likes { get; set; }
    
    [Required]
    public bool IsLike { get; set; }
}