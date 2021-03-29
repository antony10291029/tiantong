namespace Midos.Domain
{
  public interface IPagination<TEntity, TKey>: IDataMap<TEntity, TKey> where TEntity: IEntity<TKey>
  {
    int Page { get; }

    int PageSize { get; }

    int Total { get; }
  }

  public interface IPagination<TEntity>: IPagination<TEntity, long> where TEntity: IEntity<long>
  {

  }

  public class Pagination<TEntity, TKey>: DataMap<TEntity, TKey>, IPagination<TEntity, TKey> where TEntity: IEntity<TKey>
  {
    public int Page { get; init; }

    public int PageSize { get; init; }

    public int Total { get; init; }

    public Pagination(
      int page,
      int pageSize,
      int total,
      TEntity[] entities
    ): base(entities) {
      Page = page;
      PageSize = pageSize;
      Total = total;
    }
  }

  public class Pagination<TEntity>: Pagination<TEntity, long>, IPagination<TEntity>
    where TEntity: IEntity<long>
  {
    public Pagination(int page, int pageSize, int total, TEntity[] entities)
      :base(page, pageSize, total, entities) {}
  }
}
