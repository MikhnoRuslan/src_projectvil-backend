using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Projectvil.Shared.EntityFramework.Blob.Attributes;
using Projectvil.Shared.EntityFramework.Blob.Context;
using Projectvil.Shared.EntityFramework.Blob.Interfaces;
using Projectvil.Shared.EntityFramework.Blob.Models;
using Projectvil.Shared.Infrastructures.DI.Interfaces;
using Projectvil.Shared.Infrastructures.Middlewares.CustomExceptions;

namespace Projectvil.Shared.EntityFramework.Blob.Repository;

public class BlobContainer<TContainer> : IBlobContainer<TContainer>, ITransientDependence
    where TContainer : class, new()
{
    private readonly IBlobContextDatabase _blobContext;

    public BlobContainer(IBlobContextDatabase blobContext)
    {
        _blobContext = blobContext;
    }

    public async Task<Models.Blob> GetAsync(Expression<Func<Models.Blob, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _blobContext.Blobs.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IFormFile> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var blob = await _blobContext.Blobs.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        if (blob == null)
            throw new ServerException();

        using var memoryStream = new MemoryStream(blob.Content);
        var formFile = new FormFile(memoryStream, 0, memoryStream.Length, null, null)
        {
            Headers = new HeaderDictionary(),
            ContentType = blob.ContentType
        };

        return formFile;
    }

    public async Task<Guid> CreateBlobAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream();
        
        await file.CopyToAsync(memoryStream, cancellationToken);
        var content = memoryStream.ToArray();
        var contentType = file.ContentType;
        Guid containerId;

        var containerName = ContainerNameAttribute.GetContainerName<TContainer>();

        var container = await _blobContext.BlobContainers.FirstOrDefaultAsync(x => x.Name.Equals(containerName), cancellationToken);

        if (container != null)
        {
            containerId = container.Id;
        }
        else
        {
            container = new BlobContainer(Guid.NewGuid(), containerName);
            await _blobContext.BlobContainers.AddAsync(container, cancellationToken);
                
            await _blobContext.SaveAsync(cancellationToken);

            containerId = container.Id;
        }

        var blob = new Models.Blob(Guid.NewGuid(), contentType, content, containerId);
        await _blobContext.AddEntityAsync(blob, cancellationToken);
            
        await _blobContext.SaveAsync(cancellationToken);

        return blob.Id;
    }

    public async Task<Guid> UpdateAsync(Guid id, IFormFile file, CancellationToken cancellationToken = default)
    {
        var updatedModel = await _blobContext.Blobs.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        if (updatedModel == null)
            throw new ServerException();
        
        using var memoryStream = new MemoryStream();
        
        await file.CopyToAsync(memoryStream, cancellationToken);
        var content = memoryStream.ToArray();
        var contentType = file.ContentType;

        updatedModel.Content = content;
        updatedModel.ContentType = contentType;

        var result = _blobContext.Blobs.Update(updatedModel);
        
        await _blobContext.SaveAsync(cancellationToken);

        return result.Entity.Id;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var model = await _blobContext.Blobs.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (model == null)
            throw new ServerException();

        _blobContext.RemoveEntity(model);
        
       await _blobContext.SaveAsync(cancellationToken);
    }
}