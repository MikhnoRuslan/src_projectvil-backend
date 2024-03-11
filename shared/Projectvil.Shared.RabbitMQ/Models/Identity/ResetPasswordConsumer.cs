using System.ComponentModel.DataAnnotations;
using Projectvil.Shared.Helpers.EmailSender.Enums;

namespace Projectvil.Shared.RabbitMQ.Models.Identity;

public class ResetPasswordConsumer
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public EEmailSubjects SubjectType { get; set; }

    public List<object> Data { get; set; }
}