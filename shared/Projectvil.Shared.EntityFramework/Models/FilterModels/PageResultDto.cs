namespace Projectvil.Shared.EntityFramework.Models.FilterModels;

public class PageResultDto<T>
{
    private IReadOnlyList<T> _items;
    
    public long TotalCount { get; set; }
    public IReadOnlyList<T> Items
    {
        get { return _items ??= new List<T>(); }
        set => _items = value;
    }

    public PageResultDto()
    {
        
    }

    public PageResultDto(long totalCount, IReadOnlyList<T> items)
    {
        TotalCount = totalCount;
        Items = items;
    }
}