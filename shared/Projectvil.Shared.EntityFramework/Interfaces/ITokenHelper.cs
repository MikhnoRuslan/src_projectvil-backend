namespace Projectvil.Shared.EntityFramework.Interfaces;

public interface ITokenHelper
{
    Task<string> GetTokenAsync(string url, params string[] scopesInput);
}