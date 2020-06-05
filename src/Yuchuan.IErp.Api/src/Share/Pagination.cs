namespace Tiantong.Iot.Api
{
  public class Meta
  {
    public int Page { get; set; }

    public int PageSize { get; set; }

    public int Total { get; set; }
  }

  public class Pagination<T>
  {
    public Meta meta { get; set; } = new Meta();

    public T[] data { get; set; }
  }
}
