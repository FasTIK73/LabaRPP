using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPP.Database;
using RPP.Database.Mappings;

namespace CatHasPawsTests.StoragesContractsTests;

public abstract class BaseStorageContractTest : IDisposable
{
    protected CatHasPawsDbContext _context;
    protected IMapper _mapper;

    protected BaseStorageContractTest()
    {
        var options = new DbContextOptionsBuilder<CatHasPawsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CatHasPawsDbContext(options);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DataModelMappingProfile>();
        });

        _mapper = config.CreateMapper();

        //базу данных
        _context.Database.EnsureCreated();
    }

    protected void BaseSetUp()
    {
        //Очищаем все таблицы
        _context.Clients.RemoveRange(_context.Clients);
        _context.Workers.RemoveRange(_context.Workers);
        _context.Homes.RemoveRange(_context.Homes);
        _context.Tools.RemoveRange(_context.Tools);
        _context.WorkTypes.RemoveRange(_context.WorkTypes);
        _context.Reports.RemoveRange(_context.Reports);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}