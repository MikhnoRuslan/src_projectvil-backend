using Microsoft.Extensions.Configuration;

namespace Projectiv.AuthService.DomainShared.Configurations;

public class ProjectivAuthConfiguration
{
    private const string AppSetting = "appsettings.json";
    public AuthServerConfiguration AuthServer { get; set; }
    public AppConfiguration App { get; set; }
    public ConnectionStringsConfiguration ConnectionStrings { get; set; }
    
    public static ProjectivAuthConfiguration BindSettings()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var baseDirectory = Directory.GetParent(currentDir)!.ToString();
        var appSettingsPath = Directory.GetFiles(baseDirectory, AppSetting, SearchOption.AllDirectories).FirstOrDefault();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(appSettingsPath)!)
            .AddJsonFile(AppSetting)
            .AddEnvironmentVariables()
            .Build();

        var petProjectConfig = new ProjectivAuthConfiguration();
        configuration.Bind(petProjectConfig);

        return petProjectConfig;
    }
}