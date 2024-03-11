using System.ComponentModel.DataAnnotations;

namespace Projectvil.Shared.EntityFramework.Models.FilterModels;

public class FilterDefinition
{
    [Required]
    public string Name { get; init; }
    
    [Required]
    [RegularExpression(@"^(eq|ne|lt|gt|le|ge|sw|ew|con)$", ErrorMessage = "Invalid operator.")]
    public string Operator { get; init; }
    
    [Required]
    public string Value { get; init; }
}