namespace Projectvil.Shared.EntityFramework.Interfaces;

public interface IEntity<TKey>
{
    TKey Id { get; }
}

public interface IAggregateModel
{
    string CreateBy { get; set; }
    DateTime CreateOn { get; set; }
}

public interface IFullAggregateModel
{
    string UpdatedBy { get; set; }
    DateTime? UpdatedOn { get; set; }
}