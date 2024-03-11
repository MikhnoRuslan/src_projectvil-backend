using AutoMapper;
using Google.Protobuf.Collections;
using Projectiv.PetprojectsService.ApplicationShared.Dtos.Project;
using Projectiv.PetprojectsService.ApplicationShared.Inputs;
using Projectiv.PetprojectsService.Domain.Models.ProjectCard;
using Projectiv.ProjectServices.Grpc;

namespace Projectiv.PetprojectsService.Application.Mappings.Petproject;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<ProjectLikeDto, LikeResponse>()
            .ForMember(dst => dst.ProjectId, conf => conf.MapFrom(src => src.ProjectId.ToString()));
            
        CreateMap<Project, ProjectDto>()
            .ForMember(dst => dst.DocumentsIds, conf => conf.Ignore())
            .ForMember(dst => dst.Like, conf => conf.Ignore());

        CreateMap<CreateProjectInput, Project>()
            .ForMember(dst => dst.Id, conf => conf.Ignore())
            .ForMember(dst => dst.StatusName, conf => conf.Ignore())
            .ForMember(dst => dst.DomainName, conf => conf.Ignore())
            .ForMember(dst => dst.CreateBy, conf => conf.Ignore())
            .ForMember(dst => dst.CreateOn, conf => conf.Ignore())
            .ForMember(dst => dst.UpdatedBy, conf => conf.Ignore())
            .ForMember(dst => dst.UpdatedOn, conf => conf.Ignore());
        
        CreateMap<UpdateProjectInput, Project>()
            .ForMember(dst => dst.StatusName, conf => conf.Ignore())
            .ForMember(dst => dst.DomainName, conf => conf.Ignore())
            .ForMember(dst => dst.CreateBy, conf => conf.Ignore())
            .ForMember(dst => dst.CreateOn, conf => conf.Ignore())
            .ForMember(dst => dst.UpdatedBy, conf => conf.Ignore())
            .ForMember(dst => dst.UpdatedOn, conf => conf.Ignore());

        CreateMap<ProjectDto, ProjectResponse>()
            .ForMember(dst => dst.Id, conf => conf.MapFrom(src => src.Id.ToString()))
            .ForMember(dst => dst.StatusId, conf => conf.MapFrom(src => src.StatusId.ToString()))
            .ForMember(dst => dst.DomainId, conf => conf.MapFrom(src => src.DomainId.ToString()))
            .ForMember(dst => dst.ImageId, conf => conf.MapFrom(src => src.ImageId == null ? string.Empty : src.ImageId.ToString()))
            .ForMember(dst => dst.DocumentsId, conf => conf.MapFrom(src => GetDocumentsIds(src.DocumentsIds)));

        CreateMap<GetListProjectRequest, ProjectListInput>()
            .ForMember(dst => dst.Filter, conf => conf.MapFrom(src => string.IsNullOrEmpty(src.Filter) ? null : src.Filter))
            .ForMember(dst => dst.Sorting, conf => conf.MapFrom(src => string.IsNullOrEmpty(src.Sorting) ? null : src.Sorting));
        
        CreateMap<CreateProjectRequest, CreateProjectInput>()
            .ForMember(dst => dst.UserId, conf => conf.MapFrom(src => Guid.Parse(src.UserId)))
            .ForMember(dst => dst.DomainId, conf => conf.MapFrom(src => Guid.Parse(src.DomainId)))
            .ForMember(dst => dst.StatusId, conf => conf.MapFrom(src => Guid.Parse(src.StatusId)))
            .ForMember(dst => dst.ImageId,
                conf => conf.MapFrom(src => src.ImageId == null ? null : (Guid?)Guid.Parse(src.ImageId)))
            .ForMember(dst => dst.DocumentsIds, conf => conf.MapFrom(src => GetDocumentsIds(src.DocumentsId)));

        CreateMap<UpdateProjectRequest, UpdateProjectInput>()
            .ForMember(dst => dst.Id, conf => conf.MapFrom(src => Guid.Parse(src.Id)))
            .ForMember(dst => dst.UserId, conf => conf.MapFrom(src => Guid.Parse(src.UserId)))
            .ForMember(dst => dst.DomainId, conf => conf.MapFrom(src => Guid.Parse(src.DomainId)))
            .ForMember(dst => dst.StatusId, conf => conf.MapFrom(src => Guid.Parse(src.StatusId)))
            .ForMember(dst => dst.ImageId,
                conf => conf.MapFrom(src => src.ImageId == null ? null : (Guid?)Guid.Parse(src.ImageId)))
            .ForMember(dst => dst.DocumentsIds, conf => conf.MapFrom(src => GetDocumentsIds(src.DocumentsId)));
    }

    private static RepeatedField<string> GetDocumentsIds(IEnumerable<Guid> ids)
    {
        if (ids == null) return new RepeatedField<string>();
        
        var stringList = new RepeatedField<string>();
        stringList.AddRange(ids.Select(guid => guid.ToString()));

        return stringList;
    }

    private static List<Guid> GetDocumentsIds(RepeatedField<string> stringList)
    {
        return stringList is null or { Count: < 1 } 
            ? new List<Guid>() 
            : stringList.Select(str => Guid.Parse(str)).ToList();
    }
}