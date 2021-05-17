using System.Linq;

namespace Midos.SeedWork.Domain
{
  public class Repository<TEntity>: IRepository<TEntity>
    where TEntity: class, IEntity
  {
    public IUnitOfWork UnitOfWork { get; }

    private readonly EFContext _context;

    public Repository(EFContext context, IUnitOfWork unitOfWork)
    {
      _context = context;
      UnitOfWork = unitOfWork;
    }

    public virtual TEntity Add(TEntity entity)
    {
      entity.Id = 0;
      _context.Add(entity);
      _context.SaveChanges();

      return entity;
    }

    public virtual TEntity[] AddRange(TEntity[] entities)
    {
      foreach (var entity in entities) {
        entity.Id = 0;
      }

      _context.AddRange(entities);
      _context.SaveChanges();

      return entities;
    }

    public virtual TEntity Remove(long id)
    {
      var entity = _context.Find<TEntity>(id);

      _context.Remove(entity);
      _context.SaveChanges();

      return entity;
    }

    public virtual TEntity[] RemoveRange(long[] ids)
    {
      var entities = FindRange(ids);

      _context.RemoveRange(entities);
      _context.SaveChanges();

      return entities.Values.Select(kv => kv.Value).ToArray();
    }

    public virtual TEntity Update(TEntity entity)
    {
      var data = _context.Find<TEntity>(entity.Id);

      _context.Entry(data).CurrentValues.SetValues(entity);
      _context.SaveChanges();

      return data;
    }

    public virtual TEntity[] UpdateRange(TEntity[] entities)
    {
      var ids = entities.Select(entity => entity.Id).ToArray();
      var dataMap = FindRange(ids);

      foreach (var entity in entities) {
        if (dataMap.Values.ContainsKey(entity.Id)) {
          var data = dataMap.Values[entity.Id];

          _context.Entry(data).CurrentValues.SetValues(entity);
        }
      }

      return dataMap.Values.Select(kv => kv.Value).ToArray();
    }

    public virtual TEntity Find(long id)
      => _context.Find<TEntity>(id);

    public virtual DataMap<TEntity> FindRange(long[] ids)
      =>  _context.Set<TEntity>()
        .Where(entity => ids.Contains(entity.Id))
        .OrderByDescending(entity => entity.Id)
        .ToDataMap();

    public virtual DataMap<TEntity> Query(QueryParams param)
      => _context.Set<TEntity>()
        .OrderByDescending(entity => entity.Id)
        .ToDataMap();

    public virtual Pagination<TEntity> Paginate(PaginateParams param)
      => _context.Set<TEntity>()
        .OrderByDescending(entity => entity.Id)
        .Paginate(param);
  }
}
