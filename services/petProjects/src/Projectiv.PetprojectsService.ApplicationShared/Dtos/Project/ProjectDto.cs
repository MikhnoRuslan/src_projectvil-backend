namespace Projectiv.PetprojectsService.ApplicationShared.Dtos.Project;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid DomainId { get; set; }
    public string DomainName { get; set; }
    public Guid StatusId { get; set; }
    public string StatusName { get; set; }
    public string ProjectUrl { get; set; }
    public string GitUrl { get; set; }
    public Guid? ImageId { get; set; }
    public List<Guid> DocumentsIds { get; set; }
    public ProjectLikeDto Like { get; set; }
}