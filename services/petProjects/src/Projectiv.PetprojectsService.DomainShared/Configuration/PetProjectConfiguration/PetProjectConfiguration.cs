using Microsoft.Extensions.Configuration;

namespace Projectiv.PetprojectsService.DomainShared.Configuration.PetProjectConfiguration;

public class PetProjectConfiguration
{
    private const string AppSetting = "appsettings.json";
    public AppConfiguration App { get; set; }
    public AuthServerConfiguration AuthServer { get; set; }
    public PetProjectConnectionStringConfiguration ConnectionStrings { get; set; }
    
    public static PetProjectConfiguration BindSettings()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var baseDirectory = Directory.GetParent(currentDir)!.ToString();
        var appSettingsPath = Directory.GetFiles(baseDirectory, AppSetting, SearchOption.AllDirectories).FirstOrDefault();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(appSettingsPath)!)
            .AddJsonFile(AppSetting)
            .AddEnvironmentVariables()
            .Build();

        var petProjectConfig = new PetProjectConfiguration();
        configuration.Bind(petProjectConfig);

        return petProjectConfig;
    }
}