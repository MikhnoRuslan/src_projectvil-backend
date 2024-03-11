using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.WebGateway.Domain.Models;

public class TechStack : FullAggregateModel<Guid>
{
    public string Name { get; set; }
    public string IconUrl { get; set; }

    public List<ApplicationUser> Users { get; set; } = new();

    public TechStack() { }
    public TechStack(Guid id) : base(id) { }
}