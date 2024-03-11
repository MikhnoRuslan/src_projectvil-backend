using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.IdentityService.Domain.Models.Auth;

public class Client : FullAggregateModel<Guid>
{
    public string ClientId { get; set; }
    public string ClientName { get; set; }
    public string ClientSecrets { get; set; }
    public string RedirectUris { get; set; }
    public string PostLogoutRedirectUris { get; set; }
    public string Scopes { get; set; }

    public Client()
    {
        
    }

    public Client(Guid id) : base(id)
    {
        
    }
}