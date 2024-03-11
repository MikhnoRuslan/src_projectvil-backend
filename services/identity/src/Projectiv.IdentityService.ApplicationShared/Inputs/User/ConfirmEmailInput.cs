using System.ComponentModel.DataAnnotations;

namespace Projectiv.IdentityService.ApplicationShared.Inputs.User;

public class ConfirmEmailInput
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public string Token { get; set; }
}