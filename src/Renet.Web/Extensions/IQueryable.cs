using System.Linq;

namespace Renet.Web
{
  public static class IQueryableExtensions
  {
    public static IPagination<T> Paginate<T>(this IQueryable<T> query, int page, int pageSize)
    {
      return new Pagination<T> {
        Total = query.Count(),
        Data = query.Take(pageSize).Skip((page - 1) * pageSize).ToArray()
      };
    }
  }
}
