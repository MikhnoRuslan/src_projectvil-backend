using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.IdentityService.Domain.Models;
public class ExternalToken : FullAggregateModel<Guid>
{
    public string AutorizationCode { get; set; }
    public string AccessToken { get; set; }
    public int AccessTokenExpirationSeconds { get; set; }
    public DateTime AccessTokenDateCreated { get; set; }
    public string ProviderUserId { get; set; }
    public string Provider { get; set; }
}