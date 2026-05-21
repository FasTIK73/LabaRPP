using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPP.DataModels;
using RPP.Database.Models;
using RPP.Common.Exceptions;
using RPP.StoragesContracts;

namespace RPP.Database.DatabaseImplementations;

public class PostStorageContract : IPostStorageContract
{
    private readonly CatHasPawsDbContext _context;
    private readonly IMapper _mapper;

    public PostStorageContract(CatHasPawsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public List<PostDataModel> GetList()
    {
        try
        {
            var entities = _context.Posts.Where(x => x.IsActual).ToList();
            return _mapper.Map<List<PostDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public List<PostDataModel> GetPostWithHistory(string postId)
    {
        try
        {
            var entities = _context.Posts.Where(x => x.PostId == postId).OrderByDescending(x => x.ChangeDate).ToList();
            return _mapper.Map<List<PostDataModel>>(entities);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public PostDataModel? GetElementById(string id)
    {
        try
        {
            var entity = _context.Posts.FirstOrDefault(x => x.PostId == id && x.IsActual);
            return entity == null ? null : _mapper.Map<PostDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public PostDataModel? GetElementByName(string name)
    {
        try
        {
            var entity = _context.Posts.FirstOrDefault(x => x.PostName == name && x.IsActual);
            return entity == null ? null : _mapper.Map<PostDataModel>(entity);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public void AddElement(PostDataModel element)
    {
        try
        {
            var entity = _mapper.Map<PostEntity>(element);
            entity.Id = Guid.NewGuid().ToString();
            entity.PostId = element.Id;
            entity.IsActual = true;
            entity.ChangeDate = DateTime.UtcNow;
            _context.Posts.Add(entity);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public void UpdElement(PostDataModel element)
    {
        try
        {
            var oldEntity = _context.Posts.FirstOrDefault(x => x.PostId == element.Id && x.IsActual);
            if (oldEntity == null)
                throw new ElementNotFoundException(element.Id);

            oldEntity.IsActual = false;

            var newEntity = _mapper.Map<PostEntity>(element);
            newEntity.Id = Guid.NewGuid().ToString();
            newEntity.PostId = element.Id;
            newEntity.IsActual = true;
            newEntity.ChangeDate = DateTime.UtcNow;

            _context.Posts.Add(newEntity);
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

    public void DelElement(string id)
    {
        try
        {
            var entity = _context.Posts.FirstOrDefault(x => x.PostId == id && x.IsActual);
            if (entity == null)
                throw new ElementNotFoundException(id);

            entity.IsActual = false;
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

    public void ResElement(string id)
    {
        try
        {
            var entity = _context.Posts.FirstOrDefault(x => x.PostId == id && !x.IsActual);
            if (entity == null)
                throw new ElementNotFoundException(id);

            entity.IsActual = true;
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