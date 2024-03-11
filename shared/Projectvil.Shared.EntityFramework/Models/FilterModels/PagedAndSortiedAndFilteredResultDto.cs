using System.ComponentModel.DataAnnotations;
using Projectvil.Shared.EntityFramework.Interfaces;

namespace Projectvil.Shared.EntityFramework.Models.FilterModels;

public class PagedAndSortiedAndFilteredRequestDto : PagedAndSortedRequestDto, IPagedAndSortiedAndFilteredRequestDto
{
    public virtual string Filter { get; set; }
}

public class PagedAndSortedRequestDto : PagedRequestDto, IPagedAndSortedRequestDto
{
    public virtual string Sorting { get; set; }
}

public class PagedRequestDto : LimitedRequestDto, IPagedRequestDto
{
    private static int DefaultSkipCount => 0;

    [Range(0, int.MaxValue)]
    public virtual int SkipCount { get; set; } = DefaultSkipCount;
}

public class LimitedRequestDto : ILimitedRequestDto
{
    private static int DefaultMaxResultCount => 10;

    [Range(1, int.MaxValue)]
    public virtual int MaxResultCount { get; set; } = DefaultMaxResultCount;
}