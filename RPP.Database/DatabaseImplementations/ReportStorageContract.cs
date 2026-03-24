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

public class ReportStorageContract : IReportStorageContract
{
    private readonly CatHasPawsDbContext _context;
    private readonly IMapper _mapper;

    public ReportStorageContract(CatHasPawsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public List<ReportDataModel> GetList(DateTime? fromDate = null, DateTime? toDate = null)
    {
        try
        {
            var query = _context.Reports.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(x => x.WorkDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(x => x.WorkDate <= toDate.Value);

            var entities = query.ToList();
            return _mapper.Map<List<ReportDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public List<ReportDataModel> GetListByHome(string homeId)
    {
        try
        {
            var entities = _context.Reports.Where(x => x.HomeId == homeId).ToList();
            return _mapper.Map<List<ReportDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public List<ReportDataModel> GetListByWorker(string workerId)
    {
        try
        {
            var entities = _context.Reports.Where(x => x.WorkerId == workerId).ToList();
            return _mapper.Map<List<ReportDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public List<ReportDataModel> GetListByWorkType(string workTypeId)
    {
        try
        {
            var entities = _context.Reports.Where(x => x.WorkTypeId == workTypeId).ToList();
            return _mapper.Map<List<ReportDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public List<ReportDataModel> GetListByTool(string toolId)
    {
        try
        {
            var entities = _context.Reports.Where(x => x.ToolId == toolId).ToList();
            return _mapper.Map<List<ReportDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public ReportDataModel? GetElementById(string id)
    {
        try
        {
            var entity = _context.Reports.Find(id);
            return entity == null ? null : _mapper.Map<ReportDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public void AddElement(ReportDataModel element)
    {
        try
        {
            var entity = _mapper.Map<ReportEntity>(element);
            _context.Reports.Add(entity);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public void UpdateElement(ReportDataModel element)
    {
        try
        {
            var entity = _context.Reports.Find(element.Id);
            if (entity == null)
                throw new ElementNotFoundException(element.Id);

            _mapper.Map(element, entity);
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

    public void DeleteElement(string id)
    {
        try
        {
            var entity = _context.Reports.Find(id);
            if (entity == null)
                throw new ElementNotFoundException(id);

            _context.Reports.Remove(entity);
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