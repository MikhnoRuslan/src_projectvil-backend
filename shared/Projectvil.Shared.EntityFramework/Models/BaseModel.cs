using Projectvil.Shared.EntityFramework.Interfaces;

namespace Projectvil.Shared.EntityFramework.Models;

public class FullAggregateModel<TKey> : AggregateModel<TKey>
{
    public string UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }

    public FullAggregateModel()
    {
        
    }

    public FullAggregateModel(TKey id) 
        : base(id)
    {

    }
}

public class AggregateModel<TKey> : Entity<TKey>
{
    public string CreateBy { get; set; }
    public DateTime CreateOn { get; set; }
    
    protected AggregateModel()
    {

    }

    protected AggregateModel(TKey id)
        : base(id)
    {

    }
}

public class Entity<TKey> : IEntity<TKey>
{
    public TKey Id { get; protected set; }
    
    protected Entity()
    {

    }

    protected Entity(TKey id)
    {
        Id = id;
    }
}