namespace Projectiv.AuthService.ApplicationShared.Dtos;

public class TokensMappingDto
{
    public string AccessToken { get; set; }
    public string ProviderUserId { get; set; }
    public string IdToken { get; set; }
    public int ExpiresIn { get; set; }
    public bool Succeeded { get; set; } = true;
    public DateTime AccessTokenDateCreated { get; set; }
}