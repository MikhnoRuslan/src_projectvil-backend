using Microsoft.Extensions.Configuration;
using Projectiv.NotificationService.DomainShared.Configuration.PetProjectConfiguration;

namespace Projectiv.NotificationService.DomainShared.Configuration.NotificationConfiguration;

public class NotificationConfiguration
{
    private const string AppSetting = "appsettings.json";
    public AppConfiguration App { get; set; }
    public AuthServerConfiguration AuthServer { get; set; }
    public NotificationConnectionStringConfiguration ConnectionStrings { get; set; }
    
    public static NotificationConfiguration BindSettings()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var baseDirectory = Directory.GetParent(currentDir)!.ToString();
        var appSettingsPath = Directory.GetFiles(baseDirectory, AppSetting, SearchOption.AllDirectories).FirstOrDefault();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(appSettingsPath)!)
            .AddJsonFile(AppSetting)
            .AddEnvironmentVariables()
            .Build();

        var notificationConfig = new NotificationConfiguration();
        configuration.Bind(notificationConfig);

        return notificationConfig;
    }
}