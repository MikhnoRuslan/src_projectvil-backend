using Grpc.Core;
using Microsoft.Extensions.Logging;
using Projectiv.DomainServices.Grpc;
using Projectiv.GenericServices.Grpc;
using Projectiv.Language.Grpc;
using Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;
using Projectvil.Shared.EntityFramework.Translations;

namespace Projectiv.PetprojectsService.Api.Grpc;

public class DomainGrpcService : DomainPublic.DomainPublicBase
{
    private readonly ILogger<DomainGrpcService> _logger;
    private readonly IDomainAppService _domainAppService;

    public DomainGrpcService(ILogger<DomainGrpcService> logger, IDomainAppService domainAppService)
    {
        _logger = logger;
        _domainAppService = domainAppService;
    }

    public override async Task<GenericDtoMessage> GetList(LanguageMessage request, ServerCallContext context)
    {
        try
        {
            var result = await _domainAppService.GetListGrpcAsync((ELanguage)request.Language, context.CancellationToken);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}