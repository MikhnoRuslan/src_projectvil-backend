using Grpc.Core;
using Projectvil.Shared.EntityFramework.Models;

namespace Projectvil.Shared.EntityFramework.Interfaces;

public interface ICurrentUserHelper
{
    CurrentUserModel GetCurrentUserInfo();
    CurrentUserModel GetCurrentUserInfo(ServerCallContext context);
}