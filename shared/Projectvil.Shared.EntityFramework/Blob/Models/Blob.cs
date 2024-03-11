using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Projectvil.Shared.EntityFramework.Models;

namespace Projectvil.Shared.EntityFramework.Blob.Models;

public class Blob : FullAggregateModel<Guid>
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string ContentType { get; set; }

    [Required]
    public byte[] Content { get; set; }
    
    [Required]
    public Guid BlobContainerId { get; set; }
    public BlobContainer BlobContainer { get; set; }

    public Blob()
    {
        
    }

    public Blob(Guid id, [NotNull] string contentType, [NotNull] byte[] content, Guid containerId) : base(id)
    {
        Name = Guid.NewGuid().ToString();
        ContentType = contentType;
        Content = content;
        BlobContainerId = containerId;
    }
}