namespace Projectvil.Shared.EntityFramework.Interfaces;

public interface ILimitedRequestDto
{
    int MaxResultCount { get; set; }
}