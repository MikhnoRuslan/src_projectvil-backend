namespace Projectvil.Shared.EntityFramework.Interfaces;

public interface IPagedAndSortiedAndFilteredResultDto : IPagedAndSortedResultDto, IPagedResultDto, ILimitedResultDto
{
    string Filter { get; set; }
}