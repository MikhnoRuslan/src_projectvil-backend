using Microsoft.AspNetCore.Identity;
using Projectvil.Shared.EntityFramework.Interfaces;

namespace Projectvil.Shared.EntityFramework.Models;

public class ApplicationUser : IdentityUser<Guid>, IEntity<Guid>, IAggregateModel, IFullAggregateModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Country { get; set; }
    public string CreateBy { get; set; }
    public DateTime CreateOn { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    
}