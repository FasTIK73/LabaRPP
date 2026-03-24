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
using RPP.Common.Enums;
using RPP.Common.Exceptions;
using RPP.StoragesContracts;

namespace RPP.DatabaseImplementations;

public class WorkerStorageContract : IWorkerStorageContract
{
    private readonly CatHasPawsDbContext _context;
    private readonly IMapper _mapper;

    public WorkerStorageContract(CatHasPawsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public List<WorkerDataModel> GetList(bool onlyActive = true)
    {
        try
        {
            var query = _context.Workers.AsQueryable();
            if (onlyActive)
                query = query.Where(x => !x.IsDeleted);

            var entities = query.ToList();
            return _mapper.Map<List<WorkerDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public List<WorkerDataModel> GetListByPost(WorkerPost post, bool onlyActive = true)
    {
        try
        {
            var query = _context.Workers.Where(x => x.Post == post);
            if (onlyActive)
                query = query.Where(x => !x.IsDeleted);

            var entities = query.ToList();
            return _mapper.Map<List<WorkerDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public List<WorkerDataModel> GetListByBirthDate(DateTime fromDate, DateTime toDate, bool onlyActive = true)
    {
        try
        {
            var query = _context.Workers.Where(x => x.BirthDate >= fromDate && x.BirthDate <= toDate);
            if (onlyActive)
                query = query.Where(x => !x.IsDeleted);

            var entities = query.ToList();
            return _mapper.Map<List<WorkerDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public List<WorkerDataModel> GetListByHireDate(DateTime fromDate, DateTime toDate, bool onlyActive = true)
    {
        try
        {
            var query = _context.Workers.Where(x => x.HireDate >= fromDate && x.HireDate <= toDate);
            if (onlyActive)
                query = query.Where(x => !x.IsDeleted);

            var entities = query.ToList();
            return _mapper.Map<List<WorkerDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public WorkerDataModel? GetElementById(string id)
    {
        try
        {
            var entity = _context.Workers.Find(id);
            return entity == null ? null : _mapper.Map<WorkerDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public WorkerDataModel? GetElementByPhone(string phoneNumber)
    {
        try
        {
            var entity = _context.Workers.FirstOrDefault(x => x.PhoneNumber == phoneNumber);
            return entity == null ? null : _mapper.Map<WorkerDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public WorkerDataModel? GetElementByEmail(string email)
    {
        try
        {
            var entity = _context.Workers.FirstOrDefault(x => x.Email == email);
            return entity == null ? null : _mapper.Map<WorkerDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public void AddElement(WorkerDataModel element)
    {
        try
        {
            var entity = _mapper.Map<WorkerEntity>(element);
            _context.Workers.Add(entity);
            _context.SaveChanges();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Workers_PhoneNumber") == true)
        {
            throw new ElementExistsException("PhoneNumber", element.PhoneNumber);
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Workers_Email") == true)
        {
            throw new ElementExistsException("Email", element.Email);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public void UpdateElement(WorkerDataModel element)
    {
        try
        {
            var entity = _context.Workers.Find(element.Id);
            if (entity == null)
                throw new ElementNotFoundException(element.Id);

            _mapper.Map(element, entity);
            _context.SaveChanges();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Workers_PhoneNumber") == true)
        {
            throw new ElementExistsException("PhoneNumber", element.PhoneNumber);
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Workers_Email") == true)
        {
            throw new ElementExistsException("Email", element.Email);
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
            var entity = _context.Workers.Find(id);
            if (entity == null)
                throw new ElementNotFoundException(id);

            entity.IsDeleted = true;
            entity.DateOfDelete = DateTime.UtcNow;
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