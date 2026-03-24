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

public class WorkTypeStorageContract : IWorkTypeStorageContract
{
    private readonly CatHasPawsDbContext _context;
    private readonly IMapper _mapper;

    public WorkTypeStorageContract(CatHasPawsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public List<WorkTypeDataModel> GetList()
    {
        try
        {
            var entities = _context.WorkTypes.ToList();
            return _mapper.Map<List<WorkTypeDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public WorkTypeDataModel? GetElementById(string id)
    {
        try
        {
            var entity = _context.WorkTypes.Find(id);
            return entity == null ? null : _mapper.Map<WorkTypeDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public WorkTypeDataModel? GetElementByName(string name)
    {
        try
        {
            var entity = _context.WorkTypes.FirstOrDefault(x => x.WorkName == name);
            return entity == null ? null : _mapper.Map<WorkTypeDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public List<WorkTypeDataModel> GetPriceHistory(string id)
    {
        try
        {
            // Для историчности типа 4 - ищем записи с предыдущей ценой
            var workType = _context.WorkTypes.Find(id);
            if (workType == null)
                return new List<WorkTypeDataModel>();

            var history = new List<WorkTypeDataModel>();

            // Текущая запись
            history.Add(_mapper.Map<WorkTypeDataModel>(workType));

            // Здесь можно добавить логику для получения полной истории
            // В данной реализации хранится только предыдущая цена

            return history;
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public void AddElement(WorkTypeDataModel element)
    {
        try
        {
            var entity = _mapper.Map<WorkTypeEntity>(element);
            _context.WorkTypes.Add(entity);
            _context.SaveChanges();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_WorkTypes_WorkName") == true)
        {
            throw new ElementExistsException("WorkName", element.WorkName);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public void UpdateElement(WorkTypeDataModel element)
    {
        try
        {
            var entity = _context.WorkTypes.Find(element.Id);
            if (entity == null)
                throw new ElementNotFoundException(element.Id);

            // Сохраняем предыдущую цену для историчности
            if (Math.Abs(entity.PricePerUnit - element.PricePerUnit) > 0.001)
            {
                entity.PreviousPrice = entity.PricePerUnit;
                entity.PriceChangeDate = DateTime.UtcNow;
            }

            _mapper.Map(element, entity);
            _context.SaveChanges();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_WorkTypes_WorkName") == true)
        {
            throw new ElementExistsException("WorkName", element.WorkName);
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
            var entity = _context.WorkTypes.Find(id);
            if (entity == null)
                throw new ElementNotFoundException(id);

            _context.WorkTypes.Remove(entity);
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