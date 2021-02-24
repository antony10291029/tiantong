namespace System.Linq
{
  public static class IQueryableExtensions
  {
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
