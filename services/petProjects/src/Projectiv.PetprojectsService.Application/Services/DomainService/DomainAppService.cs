using AutoMapper;
using Projectiv.DomainServices.Grpc;
using Projectiv.PetprojectsService.ApplicationShared.Dtos.Domains;
using Projectiv.PetprojectsService.ApplicationShared.Inputs;
using Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;
using Projectiv.PetprojectsService.Domain.Interfaces;
using Projectiv.PetprojectsService.EntityFrameworkCore.Repositories.Interfaces;
using Projectvil.Shared.EntityFramework.Interfaces;

namespace Projectiv.PetprojectsService.Application.Services.DomainService;

public class DomainAppService : BaseReadAppService<IDomainRepository, Domain.Models.ProjectCard.Domain, DomainDto, DomainResponse, DomainListInput>, 
    IDomainAppService
{
    public DomainAppService(IMapper mapper,
        ILanguageHandler languageHandler,
        IDomainRepository repository)
        : base(mapper, 
            languageHandler,
            repository)
    {
    }
}