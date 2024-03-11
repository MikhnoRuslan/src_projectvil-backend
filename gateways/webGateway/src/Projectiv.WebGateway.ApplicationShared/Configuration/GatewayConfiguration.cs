using Microsoft.Extensions.Configuration;

namespace Projectiv.WebGateway.ApplicationShared.Configuration;

public class GatewayConfiguration
{
    private const string AppSetting = "appsettings.json";
    public AppConfiguration App { get; set; }
    public MicroservicesConfiguration Microservices { get; set; }
    public AuthServerConfiguration AuthServer { get; set; }
    
    public static GatewayConfiguration BindSettings()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var baseDirectory = Directory.GetParent(currentDir)!.ToString();
        var appSettingsPath = Directory.GetFiles(baseDirectory, AppSetting, SearchOption.AllDirectories).FirstOrDefault();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(appSettingsPath)!)
            .AddJsonFile(AppSetting)
            .AddEnvironmentVariables()
            .Build();

        var petProjectConfig = new GatewayConfiguration();
        configuration.Bind(petProjectConfig);

        return petProjectConfig;
    }
}