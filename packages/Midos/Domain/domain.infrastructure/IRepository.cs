using System;
using System.Linq;

namespace Midos.Domain
{
  public interface IRepository<TEntity, TKey> where TEntity: class, IEntity<TKey>
  {
    void SaveChanges();

    void SaveChanges(Action handler, Action<Exception> error = null);

    void Publish(string name, object data);

    IQueryable<TEntity> Query();

    TEntity Add(TEntity entity);

    TEntity Remove(TKey key);

    TEntity Remove(TEntity entity);

    TEntity Update(TEntity entity);

    TEntity Find(TKey key);

    //

    void AddRange(TEntity[] entitys);

    void RemoveRange(TKey[] key);

    void UpdateRange(TEntity[] entities);

    TEntity[] ToArray();

    IDataMap<TEntity, TKey> ToDataMap();

    IPagination<TEntity, TKey> Paginate(QueryParams param);
  }

  public interface IRepository<TEntity>: IRepository<TEntity, long>
    where TEntity: class, IEntity<long>
  {
    new IDataMap<TEntity> ToDataMap();

    new IPagination<TEntity> Paginate(QueryParams param);
  }
}
