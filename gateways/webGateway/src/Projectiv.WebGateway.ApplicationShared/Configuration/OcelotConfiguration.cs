using Microsoft.Extensions.Configuration;

namespace Projectiv.WebGateway.ApplicationShared.Configuration;

public class OcelotConfiguration
{
    private const string Ocelot = "ocelot.json";
    
    public string ServiceKey { get; set; }
    public string DownstreamPathTemplate { get; set; }
    public string DownstreamScheme { get; set; }
    public string UpstreamPathTemplate { get; set; }
    public List<string> UpstreamHttpMethod { get; set; }
    public List<HostAndPort> DownstreamHostAndPorts { get; set; }
    
    public static OcelotConfiguration BindSettings()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var baseDirectory = Directory.GetParent(currentDir)!.ToString();
        var appSettingsPath = Directory.GetFiles(baseDirectory, Ocelot, SearchOption.AllDirectories).FirstOrDefault();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(appSettingsPath)!)
            .AddJsonFile(Ocelot)
            .AddEnvironmentVariables()
            .Build();

        var petProjectConfig = new OcelotConfiguration();
        configuration.Bind(petProjectConfig);

        return petProjectConfig;
    }
}