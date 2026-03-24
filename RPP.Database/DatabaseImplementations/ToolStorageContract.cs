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

public class ToolStorageContract : IToolStorageContract
{
    private readonly CatHasPawsDbContext _context;
    private readonly IMapper _mapper;

    public ToolStorageContract(CatHasPawsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public List<ToolDataModel> GetList(bool onlyAvailable = true)
    {
        try
        {
            var query = _context.Tools.AsQueryable();
            if (onlyAvailable)
                query = query.Where(x => x.IsAvailable);

            var entities = query.ToList();
            return _mapper.Map<List<ToolDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public ToolDataModel? GetElementById(string id)
    {
        try
        {
            var entity = _context.Tools.Find(id);
            return entity == null ? null : _mapper.Map<ToolDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public ToolDataModel? GetElementByName(string name)
    {
        try
        {
            var entity = _context.Tools.FirstOrDefault(x => x.ToolName == name);
            return entity == null ? null : _mapper.Map<ToolDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public ToolDataModel? GetElementByPreviousName(string previousName)
    {
        try
        {
            var entity = _context.Tools.FirstOrDefault(x => x.PreviousToolName == previousName);
            return entity == null ? null : _mapper.Map<ToolDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public void AddElement(ToolDataModel element)
    {
        try
        {
            var entity = _mapper.Map<ToolEntity>(element);
            _context.Tools.Add(entity);
            _context.SaveChanges();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Tools_ToolName") == true)
        {
            throw new ElementExistsException("ToolName", element.ToolName);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public void UpdateElement(ToolDataModel element)
    {
        try
        {
            var entity = _context.Tools.Find(element.Id);
            if (entity == null)
                throw new ElementNotFoundException(element.Id);

            // Сохраняем предыдущее название для историчности
            if (entity.ToolName != element.ToolName)
            {
                entity.PreviousToolName = entity.ToolName;
            }

            _mapper.Map(element, entity);
            _context.SaveChanges();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Tools_ToolName") == true)
        {
            throw new ElementExistsException("ToolName", element.ToolName);
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
            var entity = _context.Tools.Find(id);
            if (entity == null)
                throw new ElementNotFoundException(id);

            _context.Tools.Remove(entity);
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