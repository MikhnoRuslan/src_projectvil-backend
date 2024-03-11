using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Projectvil.Shared.EntityFramework.Interfaces;

public interface IEntityTimestampUpdater
{
    void UpdateTimestamps(ChangeTracker changeTracker);
}