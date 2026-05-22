using AutoMapper;
using RPP.DataModels;
using RPP.Database.Models;
using RPP.StoragesContracts;

namespace RPP.Database.DatabaseImplementations;

public class ProductStorageContract : IProductStorageContract
{
    private readonly CatHasPawsDbContext _context;
    private readonly IMapper _mapper;

    public ProductStorageContract(CatHasPawsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public List<ProductDataModel> GetList(bool onlyActive = true, string? manufacturerId = null)
    {
        // Временная заглушка для 6 лабы
        return new List<ProductDataModel>();
    }
}