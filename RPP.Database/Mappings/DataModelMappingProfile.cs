using AutoMapper;
using RPP.DataModels;
using RPP.Database.Models;

namespace RPP.Database.Mappings;

public class DataModelMappingProfile : Profile
{
    public DataModelMappingProfile()
    {
        CreateMap<ClientEntity, ClientDataModel>().ReverseMap();
        CreateMap<WorkerEntity, WorkerDataModel>().ReverseMap();
        CreateMap<HomeEntity, HomeDataModel>().ReverseMap();
        CreateMap<ToolEntity, ToolDataModel>().ReverseMap();
        CreateMap<WorkTypeEntity, WorkTypeDataModel>().ReverseMap();
        CreateMap<ReportEntity, ReportDataModel>().ReverseMap();
    }
}