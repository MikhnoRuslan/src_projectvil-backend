using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.WebGateway.Domain.Models;

public class LinkToSocialMedia : FullAggregateModel<Guid>
{
    public string LinkUrl { get; set; }
    public string IconUrl { get; set; }
    
    public ApplicationUser User { get; set; }

    public LinkToSocialMedia() { }
    public LinkToSocialMedia(Guid id) : base(id) { }
}