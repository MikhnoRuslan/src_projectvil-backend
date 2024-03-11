namespace Projectiv.AuthService.ApplicationShared.Dtos;

public class MergeLocalAndExternalDto
{
    public bool Succeeded { get; set; }
    public string UserId { get; set; }
    public string ErrorText { get; set; }
}