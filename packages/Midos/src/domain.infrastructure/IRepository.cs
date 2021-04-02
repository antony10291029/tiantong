namespace Midos.Domain
{
  public interface IRepository<TEntity, TKey> where TEntity: class, IEntity<TKey>
  {
    IUnitOfWork UnitOfWork { get; }

    TEntity Add(TEntity entity);

    TEntity Remove(TKey key);

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
