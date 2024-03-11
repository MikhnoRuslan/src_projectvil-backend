using System.ComponentModel.DataAnnotations;
using Projectvil.Shared.EntityFramework.Models;

namespace Projectvil.Shared.EntityFramework.Blob.Models;

public class BlobContainer : FullAggregateModel<Guid>
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public BlobContainer()
    {
        
    }
    
    public BlobContainer(Guid id, [Required] string name) : base(id)
    {
        Name = name;
    }
}