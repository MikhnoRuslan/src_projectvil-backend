using Projectiv.AuthService.Domain.Interfaces;
using Projectiv.IdentityService.Domain.Models.Auth;
using Projectiv.IdentityService.EntityFrameworkCore.Context;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.AuthService.EntityFrameworkCore.Repositories;

public class ClientRepository : ProjectivAuthBaseRepository<Client>, IClientRepository, ITransientDependence
{
    public ClientRepository(ProjectivIdentityDbContext context) 
        : base(context)
    {
    }
}