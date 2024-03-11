using Projectiv.IdentityService.Domain.Models.Auth;
using Projectiv.IdentityService.EntityFrameworkCore.Context;
using Projectiv.IdentityService.EntityFrameworkCore.Repositories.Interfaces;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.IdentityService.EntityFrameworkCore.Repositories;

public class ClientRepository : IdentityBaseRepository<Client>, IClientRepository, ITransientDependence
{
    public ClientRepository(ProjectivIdentityDbContext context) : base(context)
    {
    }
}