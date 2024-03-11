using Microsoft.EntityFrameworkCore;
using Projectiv.NotificationService.DomainShared.Configuration.NotificationConfiguration;
using Projectvil.Shared.EntityFramework.Interfaces;

namespace Projectiv.NotificationService.EntityFrameworkCore.Context
{
    public class NotificationDbContext : DbContext
    {
        private readonly IEntityTimestampUpdater _entityTimestampUpdater;

        public NotificationDbContext(DbContextOptions<NotificationDbContext> options,
            IEntityTimestampUpdater entityTimestampUpdater)
            : base(options)
        {
            _entityTimestampUpdater = entityTimestampUpdater;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var settings = NotificationConfiguration.BindSettings();
            optionsBuilder.UseNpgsql(settings.ConnectionStrings.Notification);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Please add configuration
            base.OnModelCreating(modelBuilder);
        }
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _entityTimestampUpdater.UpdateTimestamps(ChangeTracker);
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
