using Microsoft.Extensions.Configuration;

namespace Projectiv.IdentityService.DomainShared.Configuration;

public class IdentityConfiguration
{
    private const string AppSetting = "appsettings.json";
    public AppConfiguration App { get; set; }
    public AuthServerConfiguration AuthServer { get; set; }
    public ConnectionStringsConfiguration ConnectionStrings { get; set; }
    public OpenIddictConfiguration OpenIddict { get; set; }

    public static IdentityConfiguration BindSettings()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var baseDirectory = Directory.GetParent(currentDir)!.ToString();
        var appSettingsPath = Directory.GetFiles(baseDirectory, AppSetting, SearchOption.AllDirectories).FirstOrDefault();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(appSettingsPath)!)
            .AddJsonFile(AppSetting)
            .AddEnvironmentVariables()
            .Build();

        var petProjectConfig = new IdentityConfiguration();
        configuration.Bind(petProjectConfig);

        return petProjectConfig;
    }
}