using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Midos.Domain
{
  public class Repository<TEntity, TKey>: IRepository<TEntity, TKey>
    where TEntity: class, IEntity<TKey>
  {
    protected DomainContext DomainContext;

    public Repository(DomainContext domain)
    {
      DomainContext = domain;
    }

    public void SaveChanges()
      => DomainContext.SaveChanges();

    public void SaveChanges(Action handler, Action<Exception> error = null)
      => DomainContext.SaveChanges(handler, error);

    public void Publish(string name, object data)
      => DomainContext.Publish(name, data);

    public IQueryable<TEntity> Query() => DomainContext.Set<TEntity>();

    public TEntity Add(TEntity entity) => DomainContext.Add(entity).Entity;

    public TEntity Update(TEntity entity) => DomainContext.Update(entity).Entity;

    public TEntity Remove(TKey key) => DomainContext.Remove(Find(key)).Entity;

    public TEntity Remove(TEntity entity) => DomainContext.Remove(entity).Entity;

    public TEntity Find(TKey id) => DomainContext.Find<TEntity>(id);

    public void AddRange(TEntity[] data)
      => DomainContext.AddRange(data);

    public void UpdateRange(TEntity[] data)
      => DomainContext.UpdateRange(data);

    public void RemoveRange(TKey[] keys)
      => DomainContext.RemoveRange(DomainContext.Set<TEntity>()
        .Where(entity => keys.Contains(entity.Id))
      );

    public TEntity[] ToArray()
      => DomainContext.Set<TEntity>().ToArray();

    public IDataMap<TEntity, TKey> ToDataMap()
      => DomainContext.Set<TEntity>().ToDataMap<TEntity, TKey>();

    public IPagination<TEntity, TKey> Paginate(QueryParams param)
      => DomainContext.Set<TEntity>().Paginate<TEntity, TKey>(param);
  }

  public class Repository<TEntity>: Repository<TEntity, long>, IRepository<TEntity>
    where TEntity: class, IEntity<long>
  {
    public Repository(DomainContext domain): base(domain) {}

    public new IDataMap<TEntity> ToDataMap()
      => DomainContext.Set<TEntity>().ToDataMap<TEntity>();

    public new IPagination<TEntity> Paginate(QueryParams param)
      => DomainContext.Set<TEntity>().Paginate<TEntity>(param);
  }
}
