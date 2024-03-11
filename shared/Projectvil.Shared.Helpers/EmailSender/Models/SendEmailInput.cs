using System.ComponentModel.DataAnnotations;

namespace Projectvil.Shared.Helpers.EmailSender.Models;

public class SendEmailInput
{
    [Required]
    public string Recipient { get; set; }
    
    [Required]
    public string Subject { get; set; }
    
    [Required]
    public string Body { get; set; }
}