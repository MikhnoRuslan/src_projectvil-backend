namespace Projectvil.Shared.EntityFramework.Interfaces;

public interface IPagedRequestDto
{
    int SkipCount { get; set; }
}