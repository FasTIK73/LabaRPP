using AutoMapper;
using RPP.DataModels;
using RPP.WebApi.Models.BindingModels;
using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Mappings;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<ClientBindingModel, ClientDataModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid().ToString()));
        CreateMap<WorkerBindingModel, WorkerDataModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid().ToString()))
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
        CreateMap<HomeBindingModel, HomeDataModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid().ToString()));
        CreateMap<ToolBindingModel, ToolDataModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid().ToString()));
        CreateMap<WorkTypeBindingModel, WorkTypeDataModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid().ToString()));
        CreateMap<ReportBindingModel, ReportDataModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid().ToString()))
            .ForMember(dest => dest.TotalCost, opt => opt.Ignore());

        CreateMap<ClientDataModel, ClientViewModel>();
        CreateMap<WorkerDataModel, WorkerViewModel>();
        CreateMap<HomeDataModel, HomeViewModel>();
        CreateMap<ToolDataModel, ToolViewModel>();
        CreateMap<WorkTypeDataModel, WorkTypeViewModel>();
        CreateMap<ReportDataModel, ReportViewModel>();

        CreateMap<PostDataModel, PostViewModel>()
            .ForMember(dest => dest.Configuration, opt => opt.MapFrom(src =>
                Newtonsoft.Json.JsonConvert.SerializeObject(src.ConfigurationModel)));
    }
}