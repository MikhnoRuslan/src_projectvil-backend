using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.IdentityService.Domain.Models;

public class Language : FullAggregateModel<Guid>
{
    public string Name { get; set; }

    public Language() { }
    public Language(Guid id) : base(id) { }
}