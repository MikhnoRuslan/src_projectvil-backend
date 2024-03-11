using System.Text;
using MassTransit;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Projectiv.NotificationService.ApplicationShared.Dtos;
using Projectiv.NotificationService.DomainShared;
using Projectvil.Shared.EntityFramework.Helpers;
using Projectvil.Shared.Helpers.EmailSender;
using Projectvil.Shared.Helpers.EmailSender.Enums;
using Projectvil.Shared.Helpers.EmailSender.Models;
using Projectvil.Shared.Infrastructures.Middlewares.CustomExceptions;
using Projectvil.Shared.RabbitMQ.Models.Identity;

namespace Projectiv.NotificationService.Domain.Handlers;

public sealed class EmailConsumer : 
    IConsumer<SendLinkAfterRegistrationEvent>, 
    IConsumer<ResetPasswordConsumer>
{
    private readonly IEmailSender _emailSender;
    private readonly IStringLocalizer _l;
    private readonly ILogger<EmailConsumer> _logger;

    public EmailConsumer(IEmailSender emailSender, 
        IStringLocalizer l, 
        ILogger<EmailConsumer> logger)
    {
        _emailSender = emailSender;
        _l = l;
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<SendLinkAfterRegistrationEvent> context)
    {
        var culture = context.Headers.First().Value.ToString();
        CultureHepler.Use(culture);
        
        var model = context.Message;
        var templateBody = GetTemplateBody(model.EmailSubject);

        var emailMessage = GetEmailMessage(new EmailAdaptationDto()
        {
            UserName = model.UserName,
            Email = model.Email,
            SubjectType = model.EmailSubject,
            Body = templateBody,
            Data = new List<object>() { model.Link }
        });

        await SendEmailAsync(new SendEmailInput()
        {
            Recipient = model.Email,
            Subject = _l[NotificationSubject.ConfirmEmail],
            Body = emailMessage
        });
    }
    
    public async Task Consume(ConsumeContext<ResetPasswordConsumer> context)
    {
        var culture = context.Headers.First().Value.ToString();
        CultureHepler.Use(culture);
        
        var model = context.Message;
        var templateBody = GetTemplateBody(model.SubjectType);

        var emailMessage = GetEmailMessage(new EmailAdaptationDto()
        {
            UserName = model.UserName,
            Email = model.Email,
            SubjectType = model.SubjectType,
            Body = templateBody,
            Data = model.Data
        });

        await SendEmailAsync(new SendEmailInput()
        {
            Recipient = model.Email,
            Subject = _l[NotificationSubject.ResetPasswordEmail],
            Body = emailMessage
        });
    }

    private async Task SendEmailAsync(SendEmailInput input)
    {
        try
        {
            _logger.LogInformation("Try to send email...");
            
            await _emailSender.SendEmailAsync(new SendEmailInput
            {
                //TODO change email.
                Recipient = input.Recipient,
                Subject = input.Subject,
                Body = input.Body
            });
            
            _logger.LogInformation("Email was successfully sent");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    private string GetEmailMessage(EmailAdaptationDto input)
    {
        var messageBuilder = new StringBuilder(input.Body);
        var resultBuilder = new StringBuilder();

        switch (input.SubjectType)
        {
            case EEmailSubjects.ConfirmEmail:
            case EEmailSubjects.ResetPassword:
                resultBuilder.AppendFormat(messageBuilder
                    .Replace("{{name}}", input.UserName)
                    .Replace("{{text}}", _l[NotificationMessage.ResetPasswordMessage])
                    .Replace("{{date}}", DateTime.Now.ToString("f"))
                    .Replace("\n", "")
                    .Replace("\t", "")
                    .ToString(), input.Data.ToArray());
                break;
            default:
                throw new ServerException(_l[NotificationServiceErrors.EmptySubject]);
        }

        return resultBuilder.ToString();
    }

    private string GetTemplateBody(EEmailSubjects subjectsType)
    {
        var templateName = subjectsType switch
        {
            EEmailSubjects.ConfirmEmail => NotificationServiceConstants.ConfirmationTemplateName,
            EEmailSubjects.ResetPassword => NotificationServiceConstants.ResetPasswordTemplateName,
            _ => throw new ServerException(_l[NotificationServiceErrors.EmptySubject])
        };
        
        var rootPath = Directory.GetParent(Directory.GetCurrentDirectory())!.ToString();
        var filePath = Path.Combine(
            rootPath,
            NotificationServiceConstants.AssemblyFolder,
            NotificationServiceConstants.TemplateFolderName, 
            Thread.CurrentThread.CurrentCulture.Name,
            templateName);
        
        var existing = File.Exists(filePath);
        if (!existing)
            throw new ServerException(_l[NotificationServiceErrors.CanNotFindTemplate]);
        
        var templateBody = File.ReadAllText(filePath);

        return templateBody;
    }
}