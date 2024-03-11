using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;
using Projectvil.Shared.Helpers.EmailSender.Models;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectvil.Shared.Helpers.EmailSender;

public class EmailSender : IEmailSender, ITransientDependence
{
    private const string AppSetting = "appsettings.json";
    
    public async Task SendEmailAsync(SendEmailInput input)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(AppSetting)
            .AddEnvironmentVariables()
            .Build();

        var from = new MailAddress(configuration["EmailSender:EmailFrom"]!, configuration["EmailSender:DisplayName"]);
        var to = new MailAddress(input.Recipient);
        var m = new MailMessage(from, to);
        m.Subject = input.Subject;
        m.Body = input.Body;
        m.IsBodyHtml = true;
        m.BodyEncoding = Encoding.UTF8;
        var smtp = new SmtpClient(configuration["EmailSender:Smtp"], int.Parse(configuration["EmailSender:Port"]!));
        smtp.Credentials = new NetworkCredential(configuration["EmailSender:EmailFrom"], configuration["EmailSender:Password"]);
        smtp.EnableSsl = true;
        await smtp.SendMailAsync(m);
    }
}