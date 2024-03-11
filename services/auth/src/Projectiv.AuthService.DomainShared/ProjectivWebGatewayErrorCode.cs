namespace Projectiv.AuthService.DomainShared;

public static class ProjectivWebGatewayErrorCode
{
    public static string WrongLocalEmail => "wrong_local_email";
    public static string WrongLocalPassword => "wrong_local_password";
    public static string EmailConfirmError => "email_confirm_error";
    public static string MergeError => "merge_error";

    public const string InvalidUserNameOrPassword = "InvalidUserNameOrPassword";
    public const string EmailNotConfirmed = "EmailNotConfirmed";
}