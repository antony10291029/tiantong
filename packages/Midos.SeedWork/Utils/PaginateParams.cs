namespace Midos.SeedWork
{
  public record PaginateParams: QueryParams
  {
    public int Page { get; set; }

    public int PageSize { get; set; }
  }
}
