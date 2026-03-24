using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPP.DataModels;
using RPP.Database;
using RPP.Database.Models;
using RPP.Common.Exceptions;
using RPP.StoragesContracts;

namespace RPP.DatabaseImplementations;

public class ClientStorageContract : IClientStorageContract
{
    private readonly CatHasPawsDbContext _context;
    private readonly IMapper _mapper;

    public ClientStorageContract(CatHasPawsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public List<ClientDataModel> GetList()
    {
        try
        {
            var entities = _context.Clients.ToList();
            return _mapper.Map<List<ClientDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public ClientDataModel? GetElementById(string id)
    {
        try
        {
            var entity = _context.Clients.Find(id);
            return entity == null ? null : _mapper.Map<ClientDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public ClientDataModel? GetElementByPhone(string phoneNumber)
    {
        try
        {
            var entity = _context.Clients.FirstOrDefault(x => x.PhoneNumber == phoneNumber);
            return entity == null ? null : _mapper.Map<ClientDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public ClientDataModel? GetElementByName(string name)
    {
        try
        {
            var entity = _context.Clients.FirstOrDefault(x => x.Name == name);
            return entity == null ? null : _mapper.Map<ClientDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public void AddElement(ClientDataModel element)
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

    public void UpdateElement(ClientDataModel element)
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