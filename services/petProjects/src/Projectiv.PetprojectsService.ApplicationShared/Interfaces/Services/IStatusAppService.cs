using Projectiv.GenericServices.Grpc;
using Projectiv.PetprojectsService.ApplicationShared.Dtos.Statuses;
using Projectiv.PetprojectsService.ApplicationShared.Inputs;
using Projectvil.Shared.EntityFramework.Models.FilterModels;
using Projectvil.Shared.EntityFramework.Translations;

namespace Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;

public interface IStatusAppService
{
    Task<GenericDtoMessage> GetListGrpcAsync(ELanguage language, CancellationToken cancellationToken = default);
    Task<PageResultDto<StatusDto>> GetListAsync(StatusListInput input, CancellationToken cancellationToken = default);
    Task<StatusDto> GetAsync(Guid id, ELanguage language, CancellationToken cancellationToken = default);
}