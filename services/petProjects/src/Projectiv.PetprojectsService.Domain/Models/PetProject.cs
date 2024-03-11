using Projectvil.Shared.EntityFramework.Models;

namespace Projectiv.PetprojectsService.Domain.Models
{
    public class PetProject : FullAggregateModel<Guid>
    {
        public string Name { get; set; }

        public PetProject()
        {
            
        }

        public PetProject(Guid id) : base(id)
        {
            
        }
    }
}