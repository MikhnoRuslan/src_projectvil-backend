namespace Projectvil.Shared.Infrastructures.Middlewares.CustomExceptions;

public class EntityNotFoundException : BaseException
{
    public Type EntityType { get; set; }
    public object Id { get; set; }
    
    public EntityNotFoundException()
    {

    }
    
    public EntityNotFoundException(string message)
        : base(message)
    {

    }

    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {

    }

    public EntityNotFoundException(Type entityType, object id = null, Exception innerException = null)
        : base(
            id == null
                ? $"There is no such an entity given id. Entity type: {entityType.FullName}"
                : $"There is no such an entity. Entity type: {entityType.FullName}, id: {id}",
            innerException)
    {
        EntityType = entityType;
        Id = id;
    }
}