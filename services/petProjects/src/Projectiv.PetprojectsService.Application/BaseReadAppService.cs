using AutoMapper;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Projectiv.GenericServices.Grpc;
using Projectiv.PetprojectsService.Domain.Interfaces.BaseRepository;
using Projectvil.Shared.EntityFramework.Interfaces;
using Projectvil.Shared.EntityFramework.Models.FilterModels;
using Projectvil.Shared.EntityFramework.Translations;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.PetprojectsService.Application;

public class BaseReadAppService<TRepository, TEntity, TEntityDto, TEntityGrpc, TGetListInput> : ITransientDependence
    where TRepository : IPetprojectBaseRepository<TEntity>
    where TEntity : class, IEntity<Guid>
    where TEntityDto : class
    where TEntityGrpc : IMessage
    where TGetListInput : PagedAndSortiedAndFilteredRequestDto
{
    private readonly IMapper _mapper;
    private readonly ILanguageHandler _languageHandler;
    private readonly TRepository _repository;

    public BaseReadAppService(IMapper mapper,
        ILanguageHandler languageHandler, TRepository repository)
    {
        _mapper = mapper;
        _languageHandler = languageHandler;
        _repository = repository;
    }
    
    public async Task<GenericDtoMessage> GetListGrpcAsync(ELanguage language, CancellationToken cancellationToken = default)
    {
        var models = await _repository.GetListAsync(language, cancellationToken);
        var views = _mapper.Map<List<TEntity>, List<TEntityGrpc>>(models);

        var packedViews = views.Select(view => Any.Pack(view));

        var result = new GenericDtoMessage();
        result.Data.AddRange(packedViews);
        
        return result;
    }

    public async Task<PageResultDto<TEntityDto>> GetListAsync(TGetListInput input, CancellationToken cancellationToken = default)
    {
        var language = _languageHandler.GetLanguage();
        
        var models = await _repository.GetListAsync(input, language, cancellationToken);
        var totalCount = await _repository.GetCountAsync(input, language, cancellationToken);

        var views = _mapper.Map<List<TEntity>, List<TEntityDto>>(models);

        return new PageResultDto<TEntityDto>(totalCount, views);
    }

    public async Task<TEntityDto> GetAsync(Guid id, ELanguage language, CancellationToken cancellationToken = default)
    {
        var model =  await _repository.GetAsync(x => x.Id == id, language, cancellationToken);
        
        return _mapper.Map<TEntity, TEntityDto>(model);
    }
}