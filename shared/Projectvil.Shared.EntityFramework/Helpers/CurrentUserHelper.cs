using System.IdentityModel.Tokens.Jwt;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Projectvil.Shared.EntityFramework.Interfaces;
using Projectvil.Shared.EntityFramework.Models;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectvil.Shared.EntityFramework.Helpers;

public class CurrentUserHelper : ICurrentUserHelper, ITransientDependence
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentUserModel GetCurrentUserInfo()
    {
        var claims = _httpContextAccessor.HttpContext?.User;
        var userId = claims?.Claims.FirstOrDefault(x => x.Type.Equals("userId"))?.Value;
        var userName = claims?.Claims.FirstOrDefault(x => x.Type.Equals("userName"))?.Value;
        var userEmail = claims?.Claims.FirstOrDefault(c => c.Type.Equals("userEmail"))?.Value;
        var userRole = claims?.Claims.FirstOrDefault(c => c.Type.Equals("userRoles"))?.Value;

        return new CurrentUserModel()
        {
            Id = userId != null ? Guid.Parse((ReadOnlySpan<char>)userId) : null,
            UserName = userName,
            UserEmail = userEmail,
            Roles = userRole != null
                ? userRole!.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList()
                : new List<string>(),
        };
    }

    public CurrentUserModel GetCurrentUserInfo(ServerCallContext context)
    {
        var metadata = context.RequestHeaders;
        var token = metadata.FirstOrDefault(m => m.Key.Equals("authorization", StringComparison.OrdinalIgnoreCase))?.Value;

        if (string.IsNullOrEmpty(token)) return new CurrentUserModel();
        
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token.Replace("Bearer ", ""));
        var claims = jwtToken.Claims.ToList();

        var userId = claims?.FirstOrDefault(x => x.Type.Equals("userId"))?.Value;
        var userName = claims?.FirstOrDefault(x => x.Type.Equals("userName"))?.Value;
        var userEmail = claims?.FirstOrDefault(c => c.Type.Equals("userEmail"))?.Value;
        var userRole = claims?.FirstOrDefault(c => c.Type.Equals("userRoles"))?.Value;

        return new CurrentUserModel()
        {
            Id = userId != null ? Guid.Parse((ReadOnlySpan<char>)userId) : null,
            UserName = userName,
            UserEmail = userEmail,
            Roles = userRole != null
                ? userRole!.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList()
                : new List<string>(),
        };
    }
}