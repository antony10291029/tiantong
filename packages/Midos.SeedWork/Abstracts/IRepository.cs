namespace Midos.SeedWork
{
  public interface IRepository<TEntity> where TEntity: IEntity
  {
    IUnitOfWork UnitOfWork { get; }

    TEntity Add(TEntity entity);

    TEntity[] AddRange(TEntity[] entities);

    TEntity Remove(long id);

    TEntity[] RemoveRange(long[] ids);

    TEntity Update(TEntity entity);

    TEntity[] UpdateRange(TEntity[] entities);

    TEntity Find(long id);

    DataMap<TEntity> FindRange(long[] ids);

    DataMap<TEntity> Query(QueryParams param);

    Pagination<TEntity> Paginate(PaginateParams param);
  }
}
