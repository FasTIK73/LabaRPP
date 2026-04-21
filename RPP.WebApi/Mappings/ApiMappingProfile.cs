using AutoMapper;
using RPP.DataModels;
using RPP.Database.Models;
using RPP.WebApi.Models.BindingModels;
using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Mappings;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        // BindingModel → DataModel
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

        // DataModel → ViewModel
        CreateMap<ClientDataModel, ClientViewModel>();
        CreateMap<WorkerDataModel, WorkerViewModel>();
        CreateMap<HomeDataModel, HomeViewModel>();
        CreateMap<ToolDataModel, ToolViewModel>();
        CreateMap<WorkTypeDataModel, WorkTypeViewModel>();
        CreateMap<ReportDataModel, ReportViewModel>();

        // Entity → ViewModel (для отчетов с дополнительными данными)
        CreateMap<ReportEntity, ReportViewModel>()
            .ForMember(dest => dest.HomeAddress, opt => opt.MapFrom(src => src.Home != null ? src.Home.Address : string.Empty))
            .ForMember(dest => dest.WorkTypeName, opt => opt.MapFrom(src => src.WorkType != null ? src.WorkType.WorkName : string.Empty))
            .ForMember(dest => dest.WorkerName, opt => opt.MapFrom(src => src.Worker != null ? src.Worker.FullName : string.Empty))
            .ForMember(dest => dest.ToolName, opt => opt.MapFrom(src => src.Tool != null ? src.Tool.ToolName : string.Empty));
    }
}