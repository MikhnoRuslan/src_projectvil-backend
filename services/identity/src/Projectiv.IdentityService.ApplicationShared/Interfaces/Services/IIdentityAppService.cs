using Projectiv.IdentityService.ApplicationShared.Inputs.User;

namespace Projectiv.IdentityService.ApplicationShared.Interfaces.Services;

public interface IIdentityAppService
{
    Task CreateAsync(CreateUserInput input, CancellationToken cancellationToken = default);
    Task ResetPasswordAsync(ResetPasswordInput input, CancellationToken cancellationToken = default);
    Task<string> ConfirmEmailAsync(ConfirmEmailInput input, CancellationToken cancellationToken = default);
}