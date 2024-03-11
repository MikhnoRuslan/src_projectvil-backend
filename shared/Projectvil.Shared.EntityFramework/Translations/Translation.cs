using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Projectvil.Shared.EntityFramework.Models;

namespace Projectvil.Shared.EntityFramework.Translations;

[Keyless]
public class Translation : Entity<Guid>
{
    [Required]
    public ELanguage Language { get; set; }
    
    [Required]
    public string Translate { get; set; }

    public Translation()
    {
        
    }

    public Translation(Guid id, ELanguage language, string translate)
    {
        Id = id;
        Language = language;
        Translate = translate;
    }
}