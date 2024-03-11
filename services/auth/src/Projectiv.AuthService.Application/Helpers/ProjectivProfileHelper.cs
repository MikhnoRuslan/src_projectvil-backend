using System.Security.Claims;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Projectvil.Shared.EntityFramework.Models;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.AuthService.Application.Helpers;

public class ProjectivProfileHelper : IProfileService, ITransientDependence
{
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProjectivProfileHelper(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
    {
        _userManager = userManager;
        _claimsFactory = claimsFactory;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        
        if (user == null) return;

        var userRoles = await _userManager.GetRolesAsync(user);
        var principal = await _claimsFactory.CreateAsync(user);

        var claims = principal.Claims.ToList();
        claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
        
        claims.Add(new Claim("userId", user.Id.ToString()));
        claims.Add(new Claim("userName", user.UserName!));
        claims.Add(new Claim("userEmail", user.Email!));
        claims.Add(new Claim("userRoles", string.Join(", ", userRoles)));

        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        context.IsActive = user != null;
    }
}