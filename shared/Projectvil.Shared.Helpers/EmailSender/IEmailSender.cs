using Projectvil.Shared.Helpers.EmailSender.Models;

namespace Projectvil.Shared.Helpers.EmailSender;

public interface IEmailSender
{
    Task SendEmailAsync(SendEmailInput input);
}