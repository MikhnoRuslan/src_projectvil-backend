using AutoMapper;
using Projectiv.PetprojectsService.ApplicationShared.Dtos.Statuses;
using Projectiv.PetprojectsService.Domain.Models.ProjectCard;
using Projectiv.StatusServices.Grpc;

namespace Projectiv.PetprojectsService.Application.Mappings.Petproject;

public class StatusMappingProfile : Profile
{
    public StatusMappingProfile()
    {
        CreateMap<Status, StatusDto>()
            .ForMember(dst => dst.Name, conf => conf.MapFrom(src => src.NameTranslation));
        
        CreateMap<Status, StatusResponse>()
            .ForMember(dst => dst.Id, conf => conf.MapFrom(src => src.Id.ToString()))
            .ForMember(dst => dst.Name, conf => conf.MapFrom(src => src.NameTranslation));
    }
}