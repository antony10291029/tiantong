namespace Midos.Domain
{
  public interface IQueryParams
  {
    int Page { get; }

    int PageSize { get; }

    string Query { get; }
  }

  public class QueryParams: IQueryParams
  {
    public int Page { get; init; }

    public int PageSize { get; init; }

    public string Query { get; init; }
  }
}
