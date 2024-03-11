using System.ComponentModel.DataAnnotations;
using Projectvil.Shared.Helpers.EmailSender.Enums;

namespace Projectiv.NotificationService.ApplicationShared.Dtos;

public class EmailAdaptationDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public EEmailSubjects SubjectType { get; set; }

    [Required]
    public string Body { get; set; }

    public List<object> Data { get; set; }
}