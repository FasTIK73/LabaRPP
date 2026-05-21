using AutoMapper;
using RPP.DataModels;
using RPP.Database.Models;

namespace RPP.Database.Mappings;

public class DataModelMappingProfile : Profile
{
    public DataModelMappingProfile()
    {
        CreateMap<ClientEntity, ClientDataModel>().ReverseMap();
        CreateMap<WorkerEntity, WorkerDataModel>()
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ReverseMap();
        CreateMap<HomeEntity, HomeDataModel>().ReverseMap();
        CreateMap<ToolEntity, ToolDataModel>().ReverseMap();
        CreateMap<WorkTypeEntity, WorkTypeDataModel>().ReverseMap();
        CreateMap<ReportEntity, ReportDataModel>().ReverseMap();

        CreateMap<PostEntity, PostDataModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PostId))
            .ForMember(dest => dest.ConfigurationModel, opt => opt.MapFrom(src => src.Configuration))
            .ReverseMap()
            .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Configuration, opt => opt.MapFrom(src => src.ConfigurationModel))
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}