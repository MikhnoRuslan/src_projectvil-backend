using System.ComponentModel.DataAnnotations;
using Projectvil.Shared.Helpers.EmailSender.Enums;

namespace Projectvil.Shared.RabbitMQ.Models.Identity;

public class SendLinkAfterRegistrationEvent
{
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Link { get; set; }
    
    [Required]
    public EEmailSubjects EmailSubject { get; set; }
}