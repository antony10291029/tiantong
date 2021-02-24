namespace Microsoft.AspNetCore.Mvc
{
  public interface IResult<T>
  {
    IResult<T> StatusCode(int code);

    IResult<T> Header(string key, string value);
  }
}
