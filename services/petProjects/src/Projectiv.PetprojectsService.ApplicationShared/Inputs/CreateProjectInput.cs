using System.ComponentModel.DataAnnotations;
using Projectiv.PetprojectsService.DomainShared.Configuration.ModelConfigurations.ProjectCard;

namespace Projectiv.PetprojectsService.ApplicationShared.Inputs;

public class CreateProjectInput
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    [MaxLength(ProjectCardConfiguration.MaxProjectNameLength)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(ProjectCardConfiguration.MaxProjectDescriptionLength)]
    public string Description { get; set; }
    
    [Required]
    public Guid DomainId { get; set; }
    
    [Required]
    public Guid StatusId { get; set; }
    
    [Url]
    public string ProjectUrl { get; set; }
    
    [Url]
    public string GitUrl { get; set; }
    public Guid? ImageId { get; set; }
    public List<Guid> DocumentsIds { get; set; }
}