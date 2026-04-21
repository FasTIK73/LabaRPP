using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPP.Common.Enums;
using RPP.Common.Exceptions;
using RPP.DataModels;
using RPP.Database.Models;
using RPP.StoragesContracts;

namespace RPP.Database.DatabaseImplementations;

public class HomeStorageContract : IHomeStorageContract
{
    private readonly CatHasPawsDbContext _context;
    private readonly IMapper _mapper;

    public HomeStorageContract(CatHasPawsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public List<HomeDataModel> GetList()
    {
        try
        {
            var entities = _context.Homes.ToList();
            return _mapper.Map<List<HomeDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public List<HomeDataModel> GetListByClient(string clientId)
    {
        try
        {
            var entities = _context.Homes.Where(x => x.ClientId == clientId).ToList();
            return _mapper.Map<List<HomeDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public List<HomeDataModel> GetListByStatus(HomeStatus status)
    {
        try
        {
            var entities = _context.Homes.Where(x => x.Status == status).ToList();
            return _mapper.Map<List<HomeDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public List<HomeDataModel> GetListByType(HomeType type)
    {
        try
        {
            var entities = _context.Homes.Where(x => x.Type == type).ToList();
            return _mapper.Map<List<HomeDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public HomeDataModel? GetElementById(string id)
    {
        try
        {
            var entity = _context.Homes.Find(id);
            return entity == null ? null : _mapper.Map<HomeDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public HomeDataModel? GetElementByAddress(string address)
    {
        try
        {
            var entity = _context.Homes.FirstOrDefault(x => x.Address == address);
            return entity == null ? null : _mapper.Map<HomeDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public void AddElement(HomeDataModel element)
    {
        try
        {
            var entity = _mapper.Map<HomeEntity>(element);
            _context.Homes.Add(entity);
            _context.SaveChanges();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Homes_Address") == true)
        {
            throw new ElementExistsException("Address", element.Address);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public void UpdateElement(HomeDataModel element)
    {
        try
        {
            var entity = _context.Homes.Find(element.Id);
            if (entity == null)
                throw new ElementNotFoundException(element.Id);

            _mapper.Map(element, entity);
            _context.SaveChanges();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Homes_Address") == true)
        {
            throw new ElementExistsException("Address", element.Address);
        }
        catch (ElementNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public void DeleteElement(string id)
    {
        try
        {
            var entity = _context.Homes.Find(id);
            if (entity == null)
                throw new ElementNotFoundException(id);

            _context.Homes.Remove(entity);
            _context.SaveChanges();
        }
        catch (ElementNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }
}