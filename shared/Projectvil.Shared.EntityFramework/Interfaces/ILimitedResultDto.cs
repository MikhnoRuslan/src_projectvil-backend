namespace Projectvil.Shared.EntityFramework.Interfaces;

public interface ILimitedResultDto
{
    int MaxResultCount { get; set; }
}