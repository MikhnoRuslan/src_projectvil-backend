using Projectvil.Shared.EntityFramework.Models;
using System.ComponentModel.DataAnnotations;
using Projectiv.IdentityService.Domain.Models;

namespace Projectiv.WebGateway.Domain.Models;

public class LanguageLevels : FullAggregateModel<Guid>
{
    public Language Language { get; set; }
    public ApplicationUser User { get; set; }
    [Range(0.0, 1.0)]
    public decimal Level { get; set; }

    public LanguageLevels() { }
    public LanguageLevels(Guid id) : base(id) { }
}