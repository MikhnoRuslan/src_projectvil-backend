using Grpc.Core;
using Microsoft.Extensions.Logging;
using Projectiv.GenericServices.Grpc;
using Projectiv.Language.Grpc;
using Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;
using Projectiv.StatusServices.Grpc;
using Projectvil.Shared.EntityFramework.Translations;

namespace Projectiv.PetprojectsService.Api.Grpc;

public class StatusGrpcService : StatusPublic.StatusPublicBase
{
    private readonly ILogger<StatusGrpcService> _logger;
    private readonly IStatusAppService _statusAppService;

    public StatusGrpcService(ILogger<StatusGrpcService> logger, 
        IStatusAppService statusAppService)
    {
        _logger = logger;
        _statusAppService = statusAppService;
    }

    public override async Task<GenericDtoMessage> GetList(LanguageMessage request, ServerCallContext context)
    {
        try
        {
            var result = await _statusAppService.GetListGrpcAsync((ELanguage)request.Language, context.CancellationToken);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}