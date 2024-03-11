using Microsoft.EntityFrameworkCore;
using Projectvil.Shared.EntityFramework.Blob.Models;

namespace Projectvil.Shared.EntityFramework.Blob.Context;

public interface IBlobContext 
{
    DbSet<Models.Blob> Blobs { get; }
    DbSet<BlobContainer> BlobContainers { get; }
}