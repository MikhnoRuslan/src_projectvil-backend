using System.ComponentModel.DataAnnotations.Schema;
using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.IdentityService.Domain.Models.Auth;

public class ApiResource : FullAggregateModel<Guid>
{
    public string Name { get; set; }
    public string Scopes { get; set; }

    public ApiResource()
    {
        
    }

    public ApiResource(Guid id) : base(id)
    {
        
    }
}