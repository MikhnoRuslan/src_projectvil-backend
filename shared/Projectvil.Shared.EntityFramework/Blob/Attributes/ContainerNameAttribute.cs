using System.Reflection;

namespace Projectvil.Shared.EntityFramework.Blob.Attributes;

public class ContainerNameAttribute : Attribute
{
    public string Name { get; set; }

    public ContainerNameAttribute()
    {
        
    }

    public ContainerNameAttribute(string name)
    {
        Name = name;
    }
    
    public static string GetContainerName<T>()
    {
        return GetContainerName(typeof(T));
    }

    protected virtual string GetName(Type type)
    {
        return Name;
    }
    
    private static string GetContainerName(Type type)
    {
        var nameAttribute = type.GetCustomAttribute<ContainerNameAttribute>();

        return nameAttribute == null ? type.FullName : nameAttribute.GetName(type);
    }
}