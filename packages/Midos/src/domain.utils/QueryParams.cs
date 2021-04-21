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
    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 15;

    public string Query { get; init; } = "";
  }
}
