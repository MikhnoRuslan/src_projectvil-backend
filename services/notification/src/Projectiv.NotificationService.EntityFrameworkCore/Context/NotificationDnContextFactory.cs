using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Projectiv.NotificationService.DomainShared.Configuration.NotificationConfiguration;
using Projectvil.Shared.EntityFramework.Helpers;

namespace Projectiv.NotificationService.EntityFrameworkCore.Context;

public class NotificationDbContextFactory : IDesignTimeDbContextFactory<NotificationDbContext>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NotificationDbContextFactory()
    {
        
    }
    
    public NotificationDbContextFactory(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public NotificationDbContext CreateDbContext(string[] args)
    {
        try
        {
            var settings = NotificationConfiguration.BindSettings();
            var optionsBuilder = new DbContextOptionsBuilder<NotificationDbContext>();
            optionsBuilder.UseNpgsql(settings.ConnectionStrings.Notification);

            return new NotificationDbContext(optionsBuilder.Options, new EntityTimestampUpdater(_httpContextAccessor));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}