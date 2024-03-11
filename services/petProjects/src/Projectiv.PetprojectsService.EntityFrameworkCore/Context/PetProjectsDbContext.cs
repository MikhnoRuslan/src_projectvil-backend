using Microsoft.EntityFrameworkCore;
using Projectiv.PetprojectsService.Domain.Models;
using Projectiv.PetprojectsService.Domain.Models.ProjectCard;
using Projectiv.PetprojectsService.DomainShared.Configuration.PetProjectConfiguration;
using Projectiv.PetprojectsService.EntityFrameworkCore.Context.Configurations;
using Projectvil.Shared.EntityFramework.Blob.Context;
using Projectvil.Shared.EntityFramework.Blob.Models;
using Projectvil.Shared.EntityFramework.Interfaces;
using Projectvil.Shared.EntityFramework.Translations;

namespace Projectiv.PetprojectsService.EntityFrameworkCore.Context
{
    public class PetProjectsDbContext : DbContext, IBlobContext
    {
        private readonly IEntityTimestampUpdater _entityTimestampUpdater;

        public PetProjectsDbContext(DbContextOptions<PetProjectsDbContext> options,
            IEntityTimestampUpdater entityTimestampUpdater)
            : base(options)
        {
            _entityTimestampUpdater = entityTimestampUpdater;
        }

        public DbSet<PetProject> PetProjects { get; set; }
        public DbSet<Blob> Blobs { get; set; }
        public DbSet<BlobContainer> BlobContainers { get; set; }
        public DbSet<Domain.Models.ProjectCard.Domain> Domains { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectLike> ProjectLikes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var settings = PetProjectConfiguration.BindSettings();
            optionsBuilder.UseNpgsql(settings.ConnectionStrings.PetProjects);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigurePetProject();
            modelBuilder.ConfigureTranslation();
            modelBuilder.ConfigureDomain();
            modelBuilder.ConfigureStatus();
            modelBuilder.ConfigureProject();
        }
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _entityTimestampUpdater.UpdateTimestamps(ChangeTracker);
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
