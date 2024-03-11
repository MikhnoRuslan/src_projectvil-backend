using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Projectvil.Shared.EntityFramework.Interfaces;
using Projectvil.Shared.EntityFramework.Models;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectvil.Shared.EntityFramework.Helpers;

public class EntityTimestampUpdater : IEntityTimestampUpdater, ITransientDependence
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EntityTimestampUpdater(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void UpdateTimestamps(ChangeTracker changeTracker)
    {
        var claims = _httpContextAccessor.HttpContext?.User;
        var claimUserName = claims?.Claims.FirstOrDefault(x => x.Type.Equals("userName"))?.Value;
        var userName = string.IsNullOrEmpty(claimUserName) ? "system" : claimUserName;
        var currentTime = DateTime.UtcNow;

        foreach (var entry in changeTracker.Entries())
        {
            if (entry.Entity is IAggregateModel iaggregateModel)
            {
                if (entry.State == EntityState.Added)
                {
                    iaggregateModel.CreateBy = userName;
                    iaggregateModel.CreateOn = currentTime;
                }
            }

            if (entry.Entity is IFullAggregateModel ifullAggregateModel && entry.State == EntityState.Modified)
            {
                ifullAggregateModel.UpdatedBy = userName;
                ifullAggregateModel.UpdatedOn = currentTime;
            }

            if (entry.Entity is FullAggregateModel<Guid> fullAggregateModel && entry.State == EntityState.Added)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        fullAggregateModel.CreateBy = userName;
                        fullAggregateModel.CreateOn = currentTime;
                        break;
                    case EntityState.Modified:
                        fullAggregateModel.CreateBy = userName;
                        fullAggregateModel.CreateOn = currentTime;
                        break;
                }
            }

            if (entry.Entity is AggregateModel<Guid> aggregateModel && entry.State == EntityState.Added)
            {
                aggregateModel.CreateBy = userName;
                aggregateModel.CreateOn = currentTime;
            }
        }
    }
}