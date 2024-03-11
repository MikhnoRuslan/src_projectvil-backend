using AutoMapper;
using Projectiv.DomainServices.Grpc;
using Projectiv.PetprojectsService.ApplicationShared.Dtos.Domains;

namespace Projectiv.PetprojectsService.Application.Mappings.Petproject;

public class DomainMappingProfile : Profile
{
    public DomainMappingProfile()
    {
        CreateMap<Domain.Models.ProjectCard.Domain, DomainResponse>()
            .ForMember(dst => dst.Id, conf => conf.MapFrom(src => src.Id.ToString()))
            .ForMember(dst => dst.Name, conf => conf.MapFrom(src => src.NameTranslation));

        CreateMap<Domain.Models.ProjectCard.Domain, DomainDto>()
            .ForMember(dst => dst.Name, conf => conf.MapFrom(src => src.NameTranslation));
    }
}