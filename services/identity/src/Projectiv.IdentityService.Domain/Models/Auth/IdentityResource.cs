using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.IdentityService.Domain.Models.Auth;

public class IdentityResource : FullAggregateModel<Guid>
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Claims { get; set; }

    public IdentityResource()
    {
        
    }

    public IdentityResource(Guid id) : base(id)
    {
        
    }
}