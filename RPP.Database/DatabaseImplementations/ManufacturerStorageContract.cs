using AutoMapper;
using RPP.DataModels;
using RPP.Database.Models;
using RPP.StoragesContracts;

namespace RPP.Database.DatabaseImplementations;

public class ManufacturerStorageContract : IManufacturerStorageContract
{
    private readonly CatHasPawsDbContext _context;
    private readonly IMapper _mapper;

    public ManufacturerStorageContract(CatHasPawsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public List<ManufacturerDataModel> GetList()
    {
        // Временная заглушка для 6 лабы
        return new List<ManufacturerDataModel>();
    }

    public ManufacturerDataModel? GetElementById(string id)
    {
        // Временная заглушка для 6 лабы
        return null;
    }
}