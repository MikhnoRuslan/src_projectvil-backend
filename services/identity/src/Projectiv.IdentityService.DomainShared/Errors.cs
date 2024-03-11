namespace Projectiv.IdentityService.DomainShared;

public static class Errors
{
    public const string InvalidPassword = "InvalidPassword";
    public const string ResetPasswordNotFoundUser = "User this email {0} not found";

    public const string CanNotCreateUser = "CanNotCreateUser";
    public const string NotUniqueEmail = "NotUniqueEmail";
    public const string InvalidToken = "InvalidToken";
}