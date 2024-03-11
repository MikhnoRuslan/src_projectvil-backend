using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Projectiv.PetprojectsService.ApplicationShared.Interfaces.Services;
using Projectiv.ProjectBlobServices.Grpc;

namespace Projectiv.PetprojectsService.Api.Grpc;

//[Authorize(AuthenticationSchemes = "Bearer")]
public class ProjectBlobGrpcService : ProjectBlobPublic.ProjectBlobPublicBase
{
    private readonly IPetProjectBlobService _petProjectBlobService;

    public ProjectBlobGrpcService(IPetProjectBlobService petProjectBlobService)
    {
        _petProjectBlobService = petProjectBlobService;
    }

    public override async Task<BlobResponse> Get(GetBlobRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.Id);
        var blob = await _petProjectBlobService.GetAsync(x => x.Id == id, context.CancellationToken);
        
        using var memoryStream = new MemoryStream(blob.Content);
        var fileBytes = memoryStream.ToArray();

        return new BlobResponse
        {
            File = ByteString.CopyFrom(fileBytes),
            ContentType = blob.ContentType,
            FileName = string.Empty,
            Name = string.Empty
        };
    }

    public override async Task<BlobIdResponse> Create(CreateBlobRequest request, ServerCallContext context)
    {
        var fileBytes = request.File.ToByteArray();
        using var memoryStream = new MemoryStream(fileBytes);
        var formFile = new FormFile(memoryStream, 0, memoryStream.Length, request.Name, request.FileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = request.ContentType
        };

        var result = await _petProjectBlobService.CreateAsync(formFile, context.CancellationToken);

        return new BlobIdResponse() { Id = result.ToString() };
    }

    public override async Task<BlobIdResponse> Update(UpdateBlobRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.Id);
        var fileBytes = request.File.ToByteArray();
        using var memoryStream = new MemoryStream(fileBytes);
        var formFile = new FormFile(memoryStream, 0, memoryStream.Length, request.Name, request.FileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = request.ContentType
        };

        var result = await _petProjectBlobService.UpdateAsync(id, formFile, context.CancellationToken);

        return new BlobIdResponse() { Id = result.ToString() };
    }

    public override async Task<EmptyResponse> Delete(GetBlobRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.Id);

        await _petProjectBlobService.DeleteAsync(id, context.CancellationToken);

        return new EmptyResponse();
    }
}