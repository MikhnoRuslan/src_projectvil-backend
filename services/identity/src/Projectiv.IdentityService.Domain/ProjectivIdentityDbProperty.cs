namespace Projectiv.IdentityService.Domain;

public static class ProjectivIdentityDbProperty
{
    public const string TablePrefix = "Identity.";
    public const string DbSchema = "public";
    public const int ClientIdMaxLength = 100;
    public const int ClientSecretMaxLength = 100;
    public const int NameMaxLength = 100;
    public const int DisplayNameMaxLength = 100;
}