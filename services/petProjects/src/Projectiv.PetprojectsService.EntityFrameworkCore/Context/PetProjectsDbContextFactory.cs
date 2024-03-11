using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Projectiv.PetprojectsService.DomainShared.Configuration.PetProjectConfiguration;
using Projectvil.Shared.EntityFramework.Helpers;

namespace Projectiv.PetprojectsService.EntityFrameworkCore.Context;

public class PetProjectsDbContextFactory : IDesignTimeDbContextFactory<PetProjectsDbContext>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PetProjectsDbContextFactory()
    {
        
    }
    
    public PetProjectsDbContextFactory(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public PetProjectsDbContext CreateDbContext(string[] args)
    {
        try
        {
            var settings = PetProjectConfiguration.BindSettings();
            var optionsBuilder = new DbContextOptionsBuilder<PetProjectsDbContext>();
            optionsBuilder.UseNpgsql(settings.ConnectionStrings.PetProjects);

            return new PetProjectsDbContext(optionsBuilder.Options, new EntityTimestampUpdater(_httpContextAccessor));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}