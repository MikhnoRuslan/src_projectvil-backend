using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Projectiv.IdentityService.ApplicationShared.Inputs.User;
using Projectiv.IdentityService.ApplicationShared.Interfaces.Services;
using Projectiv.IdentityService.Domain.Managers;
using Projectiv.IdentityService.DomainShared;
using Projectiv.IdentityService.DomainShared.Configuration;
using Projectiv.IdentityService.EntityFrameworkCore.Repositories.Interfaces;
using Projectvil.Shared.EntityFramework.Models;
using Projectvil.Shared.Helpers.EmailSender.Enums;
using Projectvil.Shared.Infrastructures.Constants;
using Projectvil.Shared.Infrastructures.DI.Interfaces;
using Projectvil.Shared.Infrastructures.Middlewares.CustomExceptions;
using Projectvil.Shared.RabbitMQ;
using Projectvil.Shared.RabbitMQ.Models.Identity;

namespace Projectiv.IdentityService.Application.Services;

public class IdentityAppService : IIdentityAppService, ITransientDependence
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityRepository _identityRepository;
    private readonly IdentityConfiguration _config;
    private readonly IEventBus _eventBus;
    private readonly IStringLocalizer _l;

    public IdentityAppService(UserManager<ApplicationUser> userManager,
        IIdentityRepository identityRepository,
        IEventBus eventBus,
        IStringLocalizer l)
    {
        _userManager = userManager;
        _identityRepository = identityRepository;
        _eventBus = eventBus;
        _l = l;
        _config = IdentityConfiguration.BindSettings();
    }

    public async Task CreateAsync(CreateUserInput input, CancellationToken cancellationToken = default)
    {
        await CheckIsEmailUniqueAsync(input.Email, cancellationToken);
        CheckPasswordValidity(input.Password);
        
        var user = new ApplicationUser()
        {
            UserName = input.UserName,
            Email = input.Email,
        };
        
        var identityResult = await _userManager.CreateAsync(user, input.Password);

        if (!identityResult.Succeeded)
            throw new ClientException(_l[Errors.CanNotCreateUser]);

        await _userManager.AddToRoleAsync(user, ProjectivConstants.Role.User);
        
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);
        var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
        
        var confirmationLink = $"{_config.OpenIddict.Applications.GateWayService}/api/identity-service/identity/confirm-email?userId={user.Id}&token={codeEncoded}";

        await _eventBus.PublishAsync(
            new SendLinkAfterRegistrationEvent
            {
                UserName = user.UserName,
                Email = user.Email,
                EmailSubject = EEmailSubjects.ConfirmEmail,
                Link = confirmationLink
            },
            cancellationToken);
    }

    public async Task<string> ConfirmEmailAsync(ConfirmEmailInput input, CancellationToken cancellationToken = default)
    {
        var user = await _identityRepository.GetAsync(x => x.Id == input.UserId, cancellationToken);

        var codeDecodedBytes = WebEncoders.Base64UrlDecode(input.Token);
        var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
        
        var result = await _userManager.ConfirmEmailAsync(user, codeDecoded);

        if (!result.Succeeded)
            throw new ClientException(_l[Errors.InvalidToken]);

        return _config.AuthServer.RedirectUrl;
    }

    public async Task ResetPasswordAsync(ResetPasswordInput input, CancellationToken cancellationToken = default)
    {
        var user = await _identityRepository.SingleOrDefaultAsync(x => x.NormalizedEmail == input.Email.ToUpper(),
            cancellationToken);

        if (user == null)
            throw new ClientException(string.Format(Errors.ResetPasswordNotFoundUser, input.Email));
        
        var newPassword = UserManagerPro.GeneratePassword();
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var identityResult = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        
        if (!identityResult.Succeeded)
            throw new ClientException("Something wrong");
        
        await _eventBus.PublishAsync(
            new ResetPasswordConsumer
            {
                UserName = user.UserName,
                Email = user.Email,
                SubjectType = EEmailSubjects.ResetPassword,
                Data = new List<object>() { newPassword, _config.AuthServer.RedirectUrl + "/login" }
            },
            cancellationToken);
    }

    private async Task CheckIsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _identityRepository.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (user != null)
            throw new ClientException(_l[Errors.NotUniqueEmail]);
    }

    private void CheckPasswordValidity(string password)
    {
        if (password.Length < 8)
            throw new ClientException(_l[Errors.InvalidPassword]);

        if (!Regex.IsMatch(password, "[A-Z]"))
            throw new ClientException(_l[Errors.InvalidPassword]);

        if (!Regex.IsMatch(password, "[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]"))
            throw new ClientException(_l[Errors.InvalidPassword]);

        if (!Regex.IsMatch(password, "[0-9]"))
            throw new ClientException(_l[Errors.InvalidPassword]);
    }
}