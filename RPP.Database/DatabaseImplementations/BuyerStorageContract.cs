using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPP.DataModels;
using RPP.Database.Models;
using RPP.Common.Exceptions;
using RPP.StoragesContracts;

namespace RPP.Database.DatabaseImplementations;

public class BuyerStorageContract : IBuyerStorageContract
{
    private readonly CatHasPawsDbContext _context;
    private readonly IMapper _mapper;

    public BuyerStorageContract(CatHasPawsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public List<BuyerDataModel> GetList()
    {
        try
        {
            var entities = _context.Clients.ToList();
            return _mapper.Map<List<BuyerDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public BuyerDataModel? GetElementById(string id)
    {
        try
        {
            var entity = _context.Clients.Find(id);
            return entity == null ? null : _mapper.Map<BuyerDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public BuyerDataModel? GetElementByPhone(string phoneNumber)
    {
        try
        {
            var entity = _context.Clients.FirstOrDefault(x => x.PhoneNumber == phoneNumber);
            return entity == null ? null : _mapper.Map<BuyerDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public BuyerDataModel? GetElementByName(string name)
    {
        try
        {
            var entity = _context.Clients.FirstOrDefault(x => x.Name == name);
            return entity == null ? null : _mapper.Map<BuyerDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public void AddElement(BuyerDataModel element)
    {
        try
        {
            var entity = _mapper.Map<ClientEntity>(element);
            _context.Clients.Add(entity);
            _context.SaveChanges();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Clients_PhoneNumber") == true)
        {
            throw new ElementExistsException("PhoneNumber", element.PhoneNumber);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public void UpdateElement(BuyerDataModel element)
    {
        try
        {
            var entity = _context.Clients.Find(element.Id);
            if (entity == null)
                throw new ElementNotFoundException(element.Id);

            _mapper.Map(element, entity);
            _context.SaveChanges();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Clients_PhoneNumber") == true)
        {
            throw new ElementExistsException("PhoneNumber", element.PhoneNumber);
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
            var entity = _context.Clients.Find(id);
            if (entity == null)
                throw new ElementNotFoundException(id);

            _context.Clients.Remove(entity);
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