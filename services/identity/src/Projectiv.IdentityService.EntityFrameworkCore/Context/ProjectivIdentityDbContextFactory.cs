using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Projectiv.IdentityService.DomainShared.Configuration;
using Projectvil.Shared.EntityFramework.Helpers;

namespace Projectiv.IdentityService.EntityFrameworkCore.Context;

public class ProjectivIdentityDbContextFactory : IDesignTimeDbContextFactory<ProjectivIdentityDbContext>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProjectivIdentityDbContextFactory()
    {
        
    }
    
    public ProjectivIdentityDbContextFactory(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ProjectivIdentityDbContext CreateDbContext(string[] args)
    {
        try
        {
            var settings = IdentityConfiguration.BindSettings();
            var optionsBuilder = new DbContextOptionsBuilder<ProjectivIdentityDbContext>();
            optionsBuilder.UseNpgsql(settings.ConnectionStrings.UsersDatabase);

            return new ProjectivIdentityDbContext(optionsBuilder.Options, new EntityTimestampUpdater(_httpContextAccessor));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}