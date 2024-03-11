using Projectiv.GenericServices.Grpc;
using Projectiv.PetprojectsService.ApplicationShared.Dtos.Domains;
using Projectiv.PetprojectsService.ApplicationShared.Inputs;
using Projectvil.Shared.EntityFramework.Models.FilterModels;
using Projectvil.Shared.EntityFramework.Translations;

namespace Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;

public interface IDomainAppService
{
    Task<GenericDtoMessage> GetListGrpcAsync(ELanguage language, CancellationToken cancellationToken = default);
    Task<PageResultDto<DomainDto>> GetListAsync(DomainListInput input, CancellationToken cancellationToken = default);
    Task<DomainDto> GetAsync(Guid id, ELanguage language, CancellationToken cancellationToken = default);
}