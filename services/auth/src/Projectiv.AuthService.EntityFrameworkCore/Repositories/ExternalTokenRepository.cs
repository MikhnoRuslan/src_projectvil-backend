using Projectiv.AuthService.Domain.Interfaces;
using Projectiv.IdentityService.Domain.Models;
using Projectiv.IdentityService.EntityFrameworkCore.Context;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.AuthService.EntityFrameworkCore.Repositories;

public class ExternalTokenRepository : ProjectivAuthBaseRepository<ExternalToken>, IExternalTokenRepository, ITransientDependence
{
    public ExternalTokenRepository(ProjectivIdentityDbContext context) : base(context)
    {
    }

    public async Task<bool> RemoveByCodeAsync(string code)
    {
        var accessToken = await GetAsync(x => x.AutorizationCode == code);
        if (accessToken == null) return false;
        
        await DeleteAsync(accessToken);
        return true;
    }
}