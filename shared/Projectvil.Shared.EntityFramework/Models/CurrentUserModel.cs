namespace Projectvil.Shared.EntityFramework.Models;

public class CurrentUserModel
{
    public Guid? Id { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public List<string> Roles { get; set; }
}