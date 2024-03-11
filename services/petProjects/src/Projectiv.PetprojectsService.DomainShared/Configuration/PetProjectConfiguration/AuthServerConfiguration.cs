namespace Projectiv.PetprojectsService.DomainShared.Configuration.PetProjectConfiguration;

public class AuthServerConfiguration
{
    public string Authority { get; set; }
    public string SwaggerClientId { get; set; }
    public string Secret { get; set; }
    public string RedirectUrl { get; set; }
}