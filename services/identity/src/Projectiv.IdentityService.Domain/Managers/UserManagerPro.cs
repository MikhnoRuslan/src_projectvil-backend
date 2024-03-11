using Microsoft.AspNetCore.Identity;
using Projectiv.IdentityService.DomainShared;
using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.IdentityService.Domain.Managers;

public class UserManagerPro
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserManagerPro(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public static string GeneratePassword()
    {
        const string allowedChars = IdentityServiceConsts.AllowedChars;
        const int passwordLength = IdentityServiceConsts.PasswordLength;

        var random = new Random();

        var lower = allowedChars.Where(char.IsLower).MinBy(_ => random.Next());
        var upper = allowedChars.Where(char.IsUpper).MinBy(_ => random.Next());
        var number = allowedChars.Where(char.IsDigit).MinBy(_ => random.Next());
        var special = allowedChars.Where(c => !char.IsLetterOrDigit(c)).MinBy(_ => random.Next());

        var passwordChars = Enumerable
            .Range(0, passwordLength - 4)
            .Select(_ => allowedChars[random.Next(0, allowedChars.Length)]);

        return new string(passwordChars
            .Append(lower)
            .Append(upper)
            .Append(number)
            .Append(special)
            .OrderBy(_ => random.Next())
            .ToArray());
    }
}