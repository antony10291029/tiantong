using System.Linq;

namespace Midos.SeedWork
{
  public static class IQueryableExtensions
  {
    public static DataMap<TEntity> ToDataMap<TEntity>(this IQueryable<TEntity> query)
      where TEntity: IEntity
      => new(query.ToArray());

    public static Pagination<TEntity> Paginate<TEntity>(this IQueryable<TEntity> query, PaginateParams param)
      where TEntity: IEntity
    {
      var temp = query;
      var total = query.Count();
      var data = temp.Skip((param.Page - 1) * param.PageSize).Take(param.PageSize).ToArray();

      return new(
        page: param.Page,
        pageSize: param.PageSize,
        total: total,
        entities: data
      );
    }
  }
}
