namespace Renet.Web
{
  public interface IPagination<T>
  {
    int Total { get; }

    T[] Data { get; }
  }

  public class Pagination<T> : IPagination<T>
  {
    public int Total { get; set; }

    public T[] Data { get; set; }
  }
}
