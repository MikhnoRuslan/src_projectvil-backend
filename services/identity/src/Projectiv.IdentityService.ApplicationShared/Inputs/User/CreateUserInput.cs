using System.ComponentModel.DataAnnotations;
using Projectiv.IdentityService.DomainShared.Configuration.ModelConfigurations;

namespace Projectiv.IdentityService.ApplicationShared.Inputs.User;

public class CreateUserInput
{
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(CreateUserInputConfiguration.MaxPasswordLength)]
    public string Password { get; set; }
}