using System.Collections.Generic;
using System.Linq;

namespace Midos.Domain
{
  public interface IPagination<TKey, TEntity> where TEntity: IEntity<TKey>
  {
    int Page { get; }

    int PageSize { get; }

    int Total { get; }

    TKey[] Keys { get; }

    IDictionary<TKey, TEntity> Entities { get; }
  }

  public interface IPagination<TEntity>: IPagination<long, TEntity> where TEntity: IEntity<long>
  {

  }

  public class Pagination<Tkey, TEntity>: IPagination<Tkey, TEntity> where TEntity: IEntity<Tkey>
  {
    public int Page { get; init; }

    public int PageSize { get; init; }

    public int Total { get; init; }

    public Tkey[] Keys { get; init; }

    public IDictionary<Tkey, TEntity> Entities { get; init; }

    public Pagination(
      int page,
      int pageSize,
      int total,
      TEntity[] entities
    ) {
      Page = page;
      PageSize = pageSize;
      Total = total;
      Keys = entities.Select(entity => entity.Id).ToArray();
      Entities = entities.ToDictionary(entity => entity.Id, entity => entity);
    }
  }

  public class Pagination<TEntity>: Pagination<long, TEntity>, IPagination<TEntity>
    where TEntity: IEntity<long>
  {
    public Pagination(int page, int pageSize, int total, TEntity[] entities)
      :base(page, pageSize, total, entities) {}
  }
}
