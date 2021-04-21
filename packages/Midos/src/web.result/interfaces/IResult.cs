namespace Microsoft.AspNetCore.Mvc
{
  public interface IResult<T>
  {
    string GetHeader(string key);

    int? GetStatusCode();

    IResult<T> StatusCode(int code);

    IResult<T> Header(string key, string value);
  }
}
