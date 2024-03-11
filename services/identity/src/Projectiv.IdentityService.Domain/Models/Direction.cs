using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.WebGateway.Domain.Models;

public class Direction : FullAggregateModel<Guid>
{
    public string Name { get; set; }

    public List<ApplicationUser> Users { get; set; } = new();

    public Direction() { }
    public Direction(Guid id) : base(id) { }
}