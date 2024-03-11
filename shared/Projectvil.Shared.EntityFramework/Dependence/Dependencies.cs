using Microsoft.Extensions.DependencyInjection;
using Projectvil.Shared.EntityFramework.Blob.Attributes;
using Projectvil.Shared.EntityFramework.Blob.Interfaces;
using Projectvil.Shared.EntityFramework.Blob.Repository;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectvil.Shared.EntityFramework.Dependence;

public static class Dependencies
{
    public static void AddDependencies(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        var transientDependenceTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(ITransientDependence).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        foreach (var type in transientDependenceTypes)
        {
            var interfaces = type.GetInterfaces().Where(inter => inter != typeof(ITransientDependence)).ToList();

            if (type.Name.Equals("BlobContainer`1"))
            {
                foreach (var inter in interfaces)
                {
                    if (inter.Name.Equals("IBlobContainer`1"))
                    {
                        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                        var blobs = assemblies
                            .SelectMany(assembly => assembly.GetTypes())
                            .Where(type => Attribute.IsDefined(type, typeof(ContainerNameAttribute)));

                        var domainAssembly = assemblies
                            .FirstOrDefault(assembly => assembly.FullName.Split(',').First().EndsWith("Service.Domain"));

                        foreach (var blob in blobs)
                        {
                            var blobType = Type.GetType($"{blob.FullName}, {domainAssembly.FullName}");

                            if (blobType != null)
                            {
                                var containerType =
                                    typeof(IBlobContainer<>).MakeGenericType(
                                        Type.GetType($"{blob.FullName}, {domainAssembly.FullName}"));

                                if (services.All(x => x.ServiceType != containerType))
                                {
                                    services.AddScoped(containerType, provider =>
                                    {
                                        var containerGenericType = containerType.GenericTypeArguments[0];
                                        var repositoryType =
                                            typeof(BlobContainer<>).MakeGenericType(containerGenericType);

                                        var repositoryInstance =
                                            ActivatorUtilities.CreateInstance(provider, repositoryType);

                                        return repositoryInstance;
                                    });
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var inter in interfaces)
                {
                    var interfaceName = inter.Name.Substring(1);

                    if (type.Name == interfaceName)
                        services.AddTransient(inter, type);
                }
            }
        }

        var singletonDependenceTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type =>
                typeof(ISingletonDependence).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        foreach (var type in singletonDependenceTypes)
        {
            var interfaces = type.GetInterfaces().Where(inter => inter != typeof(ISingletonDependence));

            foreach (var inter in interfaces)
            {
                var interfaceName = inter.Name.Substring(1);

                if (type.Name == interfaceName)
                    services.AddSingleton(inter, type);
            }
        }

        var scopeDependenceTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IScopeDependence).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        foreach (var type in scopeDependenceTypes)
        {
            var interfaces = type.GetInterfaces().Where(inter => inter != typeof(IScopeDependence));

            foreach (var inter in interfaces)
            {
                var interfaceName = inter.Name.Substring(1);
                if (type.Name == interfaceName)
                    services.AddScoped(inter, type);
            }
        }
    }
}