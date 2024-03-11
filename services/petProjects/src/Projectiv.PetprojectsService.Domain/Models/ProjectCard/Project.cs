using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Projectiv.PetprojectsService.DomainShared.Configuration.ModelConfigurations.ProjectCard;
using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.PetprojectsService.Domain.Models.ProjectCard;

public class Project : FullAggregateModel<Guid>
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
    public Domain Domain { get; set; }
    
    [Required]
    public Guid StatusId { get; set; }
    public Status Status { get; set; }
    
    public string ProjectUrl { get; set; }
    public string GitUrl { get; set; }
    public Guid? ImageId { get; set; }

    public ICollection<ProjectDocument> ProjectDocuments { get; set; }
    public ICollection<ProjectLike> ProjectLikes { get; set; }

    [NotMapped]
    public string StatusName { get; set; }
    
    [NotMapped]
    public string DomainName { get; set; }

    public Project()
    {
        
    }

    public Project(Guid id) : base(id)
    {
        
    }
}