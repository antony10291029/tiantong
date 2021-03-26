using Midos.Domain;

namespace System.Linq
{
  public static class IQueryableExtensions
  {

    public static IPagination<TKey, TEntity> Paginate<TKey, TEntity>(this IQueryable<TEntity> query, int page, int pageSize)
      where TEntity: IEntity<TKey>
    {
      var temp = query;
      var total = query.Count();
      var data = temp.Skip((page - 1) * pageSize).Take(pageSize).ToArray();

      return new Pagination<TKey, TEntity>(
        page: page,
        pageSize: pageSize,
        total: total,
        entities: data
      );
    }

    public static IPagination<TEntity> PaginateNext<TEntity>(this IQueryable<TEntity> query, int page, int pageSize)
      where TEntity: IEntity<long>
    {
      var temp = query;
      var total = query.Count();
      var data = temp.Skip((page - 1) * pageSize).Take(pageSize).ToArray();

      return new Midos.Domain.Pagination<TEntity>(
        page: page,
        pageSize: pageSize,
        total: total,
        entities: data
      );
    }

    // @todo remove
    public static Pagination<T> Paginate<T>(this IQueryable<T> query, int page, int pageSize)
    {
      var cache = query;
      var total = query.Count();
      var data = cache.Skip((page - 1) * pageSize).Take(pageSize).ToArray();
      var pagination = new Pagination<T>();

      pagination.data = data;
      pagination.meta.Page = page;
      pagination.meta.Total = total;
      pagination.meta.PageSize = pageSize;

      return pagination;
    }
  }
}
