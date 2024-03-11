namespace Projectvil.Shared.EntityFramework.Interfaces;

public interface IPagedAndSortiedAndFilteredRequestDto : IPagedAndSortedRequestDto, IPagedRequestDto, ILimitedRequestDto
{
    string Filter { get; set; }
}