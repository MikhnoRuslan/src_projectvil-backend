using AutoMapper;
using Projectiv.PetprojectsService.ApplicationShared.Dtos.Statuses;
using Projectiv.PetprojectsService.ApplicationShared.Inputs;
using Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;
using Projectiv.PetprojectsService.Domain.Models.ProjectCard;
using Projectiv.PetprojectsService.EntityFrameworkCore.Repositories.Interfaces;
using Projectiv.StatusServices.Grpc;
using Projectvil.Shared.EntityFramework.Interfaces;

namespace Projectiv.PetprojectsService.Application.Services.StatusService;

public class StatusAppService : BaseReadAppService<IStatusRepository, Status, StatusDto, StatusResponse, StatusListInput>, IStatusAppService
{
    public StatusAppService(IMapper mapper,
        ILanguageHandler languageHandler, 
        IStatusRepository repository)
        : base(mapper, 
            languageHandler, 
            repository)
    {
    }
}