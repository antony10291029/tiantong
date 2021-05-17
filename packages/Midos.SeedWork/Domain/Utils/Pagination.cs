namespace Midos.SeedWork.Domain
{
  public class Pagination<TEntity>: DataMap<TEntity> where TEntity: IEntity
  {
    public int Page { get; }

    public int PageSize { get; set; }

    public long Total { get; set; }

    public Pagination(int page, int pageSize, int total, TEntity[] entities)
      : base(entities)
    {
      Page = page;
      PageSize = pageSize;
      Total = total;
    }
  }
}
