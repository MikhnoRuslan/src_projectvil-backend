using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.WebGateway.Domain.Models;

public class TagByInterest : FullAggregateModel<Guid>
{
    public string Name { get; set; }
    public string IconUrl { get; set; }

    public List<ApplicationUser> Users { get; set; } = new();

    public TagByInterest() { }
    public TagByInterest(Guid id) : base(id) { }
}